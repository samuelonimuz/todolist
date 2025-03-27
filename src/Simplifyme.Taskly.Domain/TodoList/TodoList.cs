using Domaincrafters.Domain;
using Simplifyme.Taskly.Domain.User;



namespace Simplifyme.Taskly.Domain.TodoList;

    public sealed class TodoListId(string? id = "") : UuidEntityId(id);


    public class TodoList : Entity<TodoListId>
    {
        public string? Title { get; private set; }
        public UserId? UserId { get; }
        private readonly List<Task> Tasks;

        public bool IsLocked { get; private set; }


        public TodoList(
            TodoListId id, 
            string title,
            UserId? userId,
            List<Task> tasks
            ) : base(id)
        {
            SetTitle(title);
            UserId = userId;
            Tasks = tasks ?? new List<Task>();

            ValidateState();
        }

        static public TodoList Create(UserId? userId, string title)
        {
            var tasks = new List<Task>();
            var todoList = new TodoList(new TodoListId(), title, userId, tasks);

            todoList.ValidateState();

            DomainEventPublisher
                .Instance
                .Publish(TodoListCreated.Create(todoList.Id.Value.ToString(), title));

            return todoList;
    
        }

        public TaskId AddTask(string description, TaskId? taskId = null, bool isDone = false)
        {
            EnsureNotLocked();

            var task = Task.Create(description, isDone, taskId);

            EnsureValidTask(task);

            Tasks.Add(task);

            return task.Id;
        }

        public Task RemoveTask(TaskId taskId)
        {
            EnsureNotLocked();

            var index = Tasks.FindIndex(task => task.Id.Equals(taskId));

            if (index == -1)
            {
                throw new InvalidOperationException(
                    $"Task with id {taskId} not found in TodoList with id {Id}."
                );
            }

            var removedTask = Tasks[index];
            Tasks.RemoveAt(index);

            return removedTask;
        }

        public Task? GetTaskAtIndex(int index)
        {
            return Tasks[index];
        }

        public void CompleteTask(TaskId taskId)
        {
            EnsureNotLocked();

            var task = Tasks.FirstOrDefault(t => t.Id.Equals(taskId));
            if (task == null)
                throw new InvalidOperationException("Task not found.");

            task.Complete();
        }

        public void Lock()
        {
            IsLocked = true;
        }

        public void Unlock()
        {
            IsLocked = false;
        }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            Title = title;
        }

        public override void ValidateState()
        {
            Tasks.ForEach(task => task.ValidateState());
            ValidateDescription();
        }

        public void ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }
        }

        private void EnsureNotLocked()
        {
            if (IsLocked)
            {
                throw new InvalidOperationException("Cannot modify a locked TodoList.");
            }
        }

        private void EnsureValidTask(Task taskToCheck)
        {
            if (Tasks.Any(task => task.Id.Equals(taskToCheck.Id)))
            {
                throw new InvalidOperationException(
                    $"Task with id {taskToCheck.Id} already exists in TodoList with id {Id}."
                );
            }
        }
    }
    

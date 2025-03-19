using Domaincrafters.Domain;



namespace Simplifyme.Taskly.Domain.TodoList;

    public sealed class TodoListId(string? id = "") : UuidEntityId(id);


    public class TodoList : Entity<TodoListId>
    {
        public TodoListId? Id { get; }
        public string Title { get; private set; }
        public Guid UserId { get; }
        private readonly List<Task> _tasks;
        public IReadOnlyCollection<Task> Tasks => _tasks.AsReadOnly();
        public bool IsLocked { get; private set; }


        public TodoList(
            TodoListId id, 
            string title,
             Guid userId
             ) : base(id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            SetTitle(title);
            UserId = userId;
            _tasks = new List<Task>();
            IsLocked = false;

            DomainEventPublisher
                .Instance
                .Publish(TodoListCreated.Create(Id.Value.ToString(), Title));
        }

        public void AddTask(Task task)
        {
            if (IsLocked)
                throw new InvalidOperationException("Cannot modify a locked TodoList.");

            if (task == null)
                throw new ArgumentNullException(nameof(task));

            _tasks.Add(task);
        }

        public void RemoveTask(TaskId taskId)
        {
            if (IsLocked)
                throw new InvalidOperationException("Cannot modify a locked TodoList.");

            var task = _tasks.FirstOrDefault(t => t.Id.Equals(taskId));
            if (task != null)
                _tasks.Remove(task);
        }

        public void CompleteTask(TaskId taskId)
        {
            if (IsLocked)
                throw new InvalidOperationException("Cannot modify a locked TodoList.");

            var task = _tasks.FirstOrDefault(t => t.Id.Equals(taskId));
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
            throw new NotImplementedException();
        }

        public void ValidateDescription()
        {
            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new ArgumentException("Title cannot be empty.");
            }
        }
    }
    

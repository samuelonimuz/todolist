using Simplifyme.Taskly.Domain.Shared;
namespace Simplifyme.Taskly.Domain.TodoList;
    
    public class TodoListCreated : TasklyDomainEvent
    {
        public string TodoListId { get; }
        public string Title { get; }

        public static TodoListCreated Create(string todoListId, string title)
        {
            if (string.IsNullOrWhiteSpace(todoListId))
                throw new ArgumentException("TodoListId cannot be empty.", nameof(todoListId));
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title cannot be empty.", nameof(title));

            return new TodoListCreated(todoListId, title, DateTime.UtcNow);
        }

        private TodoListCreated(string todoListId, string title, DateTime occurredOn)
            : base(nameof(TodoListCreated))
        {
            TodoListId = todoListId;
            Title = title;
        }
    }
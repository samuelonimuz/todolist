using Domaincrafters.Domain;

namespace Simplifyme.Taskly.Domain.TodoList;

public interface ITodoListRepository
{
    Task<IEnumerable<TodoList>> AllByYserIdAsync(Guid userId);
    Task SaveAsync(TodoList todoList);
    Task<TodoList?> GetByIdAsync(TodoListId todoListId);
}

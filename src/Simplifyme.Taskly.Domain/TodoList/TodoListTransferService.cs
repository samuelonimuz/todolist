using Domaincrafters.Domain;
using System.Threading.Tasks;

namespace Simplifyme.Taskly.Domain.TodoList;

public class TodoListTransferService
{
    private readonly ITodoListRepository _todoListRepository;

    public TodoListTransferService(ITodoListRepository todoListRepository)
    {
        _todoListRepository = todoListRepository;
    }

    public async System.Threading.Tasks.Task TransferTaskAsync(TodoListId sourceListId, TodoListId targetListId, TaskId taskId)
    {
        var sourceTodoList = await _todoListRepository.GetByIdAsync(sourceListId)
        ?? throw new InvalidOperationException($"TodoList with id {sourceListId} not found.");

        var targetTodoList = await _todoListRepository.GetByIdAsync(targetListId)
        ?? throw new InvalidOperationException($"TodoList with id {targetListId} not found.");

        var task = sourceTodoList.RemoveTask(taskId);
        
        targetTodoList.AddTask(task.GetDescription(), task.GetId(), task.GetIsDone());
        _todoListRepository.SaveAsync(sourceTodoList);
        _todoListRepository.SaveAsync(targetTodoList);

        DomainEventPublisher
            .Instance
            .Publish(TaskTransferred.Create(taskId, sourceListId, targetListId));


    }
}
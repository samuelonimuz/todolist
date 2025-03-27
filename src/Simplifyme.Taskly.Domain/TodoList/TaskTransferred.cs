using Simplifyme.Taskly.Domain.Shared;
namespace Simplifyme.Taskly.Domain.TodoList;

public class TaskTransferred : TasklyDomainEvent
{
    private TaskId TaskId { get; }
    private TodoListId SourceListId { get; }
    private TodoListId TargetListId { get; }

    public static TaskTransferred Create(TaskId taskId, TodoListId sourceListId, TodoListId targetListId)
    {
        if (taskId == null)
            throw new ArgumentNullException(nameof(taskId));
        if (sourceListId == null)
            throw new ArgumentNullException(nameof(sourceListId));
        if (targetListId == null)
            throw new ArgumentNullException(nameof(targetListId));

        return new TaskTransferred(taskId, sourceListId, targetListId, DateTime.UtcNow);
    }

    private TaskTransferred(TaskId taskId, TodoListId sourceListId, TodoListId targetListId, DateTime occurredOn)
        : base(nameof(TaskTransferred))
    {
        TaskId = taskId;
        SourceListId = sourceListId;
        TargetListId = targetListId;
    }


}
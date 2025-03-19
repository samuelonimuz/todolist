using System.ComponentModel;
using Domaincrafters.Domain;

namespace Simplifyme.Taskly.Domain.TodoList;

public sealed class TaskId(string? id = "") : UuidEntityId(id);

public class Task : Entity<TaskId>
{
    private string Description { get; set; }
    private bool IsDone { get; set; }

    public Task(
        TaskId id,
        string description, 
        bool isDone
    ) : base(id)
    {
        Description = description;
        IsDone = isDone;
    }

    public static Task Create(string description, bool? isDone, TaskId? taskId)
    {
        taskId ??= new TaskId();

        Task task = new(taskId, description, isDone ?? false);

        task.ValidateState();

        return task;
    }

    public TaskId GetId()
    {
        return Id;
    }

    public string GetDescription()
    {
        return Description;
    }

    public bool GetIsDone()
    {
        return IsDone;
    }

    public void Complete()
    {
        IsDone = true;
    }

    public override void ValidateState()
    {
        if(string.IsNullOrWhiteSpace(Description)) throw new ArgumentException("Description cannot be empty");
    }


}

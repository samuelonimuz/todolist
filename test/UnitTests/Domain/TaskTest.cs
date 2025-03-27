using Simplifyme.Taskly.Domain.TodoList;

namespace UnitTests.Domain;

public class TaskTest {


    [Fact]
    public void Create_WithValidData_Task() {
        //Arrange
        Simplifyme.Taskly.Domain.TodoList.Task task = Simplifyme.Taskly.Domain.TodoList.Task.Create("My Task", false, new TaskId());

        //Assert
        Assert.NotNull(task);
        Assert.Equal("My Task", task.GetDescription());
        Assert.False(task.GetIsDone());
    }
}
using Microsoft.VisualBasic;
using Simplifyme.Taskly.Domain.TodoList;
using Simplifyme.Taskly.Domain.User;

namespace UnitTests.Domain;

public class TodoListTests
{
    [Fact]
    public void Create_WithValidData_TodoList()
    {
        //Arrange
        UserId userId = new UserId();
        var todoList = TodoList.Create(userId, "My Todo List");

        //Assert
        Assert.NotNull(todoList);
        Assert.Equal("My Todo List", todoList.Title);
        Assert.Equal(userId, todoList.UserId);
    }

    [Fact]
    public void AddTask_WithValidData_ShouldAddTask()
    {
        //Arrange
        UserId userId = new UserId();
        TodoList todoList = TodoList.Create(userId, "My Todo List");

        //Act
        TaskId taskId = todoList.AddTask("My Task");

        //Assert
        Assert.NotNull(taskId);
    }

    [Fact]
    public void Retrieve_Task_By_Index_returns_Correct_Task()
    {
        //Arrange
        UserId userId = new UserId();
        var todoList = TodoList.Create(userId, "My Todo List");

        //Act
        todoList.AddTask("My Task");

        //Assert
        var task = todoList.GetTaskAtIndex(0);
        Assert.NotNull(task);
        Assert.Equal("My Task", task.GetDescription());
            
    }
}
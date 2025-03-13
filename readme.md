# Lab: Building Taskly in .NET with DDD & Clean Architecture 🏗️

## Prerequisites 📋

- IDE: **Visual studio code**
- Vscode extensions:
  - C# Dev Kit
- .NET 8

You are free to use any IDE you want, but support will be given for **Visual Studio Code**.

## Setup ⚙️

⚠️ Important: Do not start the lab before completing all of these steps:

- Clean the dotnet project.
```bash
donet clean
```

- Build the dotnet project.
```bash
dotnet build
```

- Restore the tools.
```bash
dotnet tool restore
```

- Run the unit tests.
```bash
dotnet test
```

If any step fails, please reach out to your lecturer or fellow students for help! 🤝

## Introduction 🎯

In this lab, you will build a task management application called "Taskly" using Domain-Driven Design (DDD) and Clean Architecture principles. 

_Except for CQRS in entity framework (part III), this lab does not contain any extra concepts compared to the Deno version of the app._

## Part I: Domain Layer Implementation 🔨

Let's build the foundation of our application by implementing the Domain Layer. We'll create core domain entities, events, repositories, and services.

### Tips 💡
- This lab ports the **Taskly** app from **Deno** to **.NET**. <br>If you need clarity on any entities or use cases, check the Deno version!
- The codebase includes a complete **CreateUser** flow - use it as your implementation guide.

### Step 1: Core Domain Entities 📦

Create the following domain entities:

1. Implement the `TodoList` aggregate root
2. Implement the `Task` entity
3. Create ID primary types: `TodoListId`, `TaskId`

Remember to:
- Make entities encapsulated with proper validation
- Use value objects for identifiers
- Follow the principles of DDD for aggregate design

**Implementation Requirements:**

The `TodoList` aggregate should:
- Store a title, user ID, and collection of tasks
- Support adding tasks
- Support completing tasks
- Support removing tasks
- Have a locking mechanism (locked lists cannot be modified)
- Validate its state

The `Task` entity should:
- Store a description and completion status
- Support being marked as completed
- Validate its description is not empty

### Step 2: Domain Events 📣
`UserCreated`: Triggered when an user is created. <br>This event is already implemented in the codebase. You can use this as a reference for your own implementation.

Implement the following domain events:

1. `TodoListCreated`: Triggered when a new todo list is created
    - To emit a domain event
    ```csharp
        DomainEventPublisher
            .Instance
            .Publish(TodoListCreated.Create(todoList.Id.Value, todoList.Title));
    ```
2. `TaskTransfered`: Triggered when a task moves between lists. (Optional)

**Each domain event should**:
- Extend a base `TasklyDomainEvent` class
- Include **relevant** information about the event
- Have a static factory method named `Create`

### Step 3: Repository Interfaces 💾

Create the repository interface:

1. `ITodoListRepository`: For persisting and retrieving todo lists

### Step 4: Domain Services 🛠️

Implement the `TodoListTransferService` which:
- Takes a source list, target list, and task ID
- Moves a task from one list to another
- Publishes a domain event when a task is transferred
- A domain service should be stateless and is allowed to use repositories.

### Step 5: Unit Tests 🧪
Write unit tests for the following:
1. `TodoList` aggregate
2. `Task` entity

run the following command to run the tests:
```bash
dotnet test
```

## Part II: Application Layer Implementation 🔄

In the second part, you will implement the application layer of the Taskly application. <br>This includes creating use cases, query objects, and interfaces (ports) for the application layer.

### Step 1: Query Data Objects 📄

Read queries do not return domain entities. Instead, they return data objects that are used for external presentation.

Create the following Data classes that will be used as the result of the queries:

1. `TodoListData`: Represents a todo list for external presentation
    - Contains a list of tasks (taskdata)
    - Contains a full user data object
    - Contains an Id
2. `TaskData`: Represents a task for external presentation
   - Contains a description
   - Contains a completion status 
   - Contains an ID

### Step 2: Query Interfaces 🔍

Create the following query interfaces in the `Contracts/Ports/Queries` folder:

1. `IAllTodoListsQuery`: For retrieving all todo lists

### Step 3: Query Filters 🎯

To make the queries more flexible, we will create reusable filters which can be used in the queries.

Implement expression filters for querying todo lists:
- `TodoListDataExpressions.ByUserId`: Filter todo lists by user ID

### Step 4: Use Cases 📝

All use cases should:
- Include appropriate logging
- Handle exceptions appropriately

Implement the following use cases:

(Remember id's in use case input classes are strings and not yet converted to the ID primary types (TaskId, TodoListId)).

1. **Create Todo List**
   - Input: User ID and optional title
   - Output: ID of the created list
   - Actions: Create a new todo list, save it to the repository.

2. **Add Task to Todo List**
   - Input: Todo list ID and task description
   - Output: Task ID and todo list ID
   - Actions: Find the todo list, create a new task, add it to the list, save changes

3. **Complete Task**
   - Input: Todo list ID and task ID
   - Actions: Find the todo list, mark task as complete, save changes

4. **Get All Todo Lists by User ID**
   - Input: User ID
   - Output: List of todo lists for the user
   - Actions: Use a query interface to retrieve all todo lists, apply filter the filter created earlier.

5. **Suspend User**
    - Input: User ID
    - Actions: Suspend the user and lock all their todo lists.
      - 'todoList.Lock()'

6. **Transfer Task to Other List** (optional)
   - Input: Task ID, source list ID, target list ID
   - Actions: Use the domain service to transfer task, save changes

### Step 5: Testing ✅

For each use case, write a happy path test and a sad path test.

### Step 6: Use case registration with DI 💉
In **Main**, register the use cases in the **Dependency Injection** container. <br>This is done in the file **UseCaseServices.cs**.

## PART III: Infrastructure Layer Implementation 🏭

!STOP! 

**Only do this part after you have followed the demo about infrastructure in dotnet** by the lecturers.

In this part, you will implement the infrastructure layer of the application.<br> This includes the following:

- Implementing the **Persistence** module
- Implementing the **WebAPI** module
- Implementing the **Messaging** module

From now on, make sure the **docker** containers are running. <br>You can do this by running the following command in the root of the project:
```bash
docker compose -f config/docker-compose.yml up -d --remove-orphans
```

### Step 0: fixing hardcoded credentials 🔑

Take a look at **appsettings.Development.json**. 

The database username and password are supposed to be read from a secrets file (only for development environment).<br>

Create a user secrets file and add the following secrets:
- database username
- database password

Here is an example of how to do this:

```bash
# Create a user secrets file
dotnet user-secrets init --project src/Simplifyme.Taskly.Main

# Add the secrets to the user secrets file
dotnet user-secrets set "Database:Username" "taskly" --project src/Simplifyme.Taskly.Main

dotnet user-secrets set "Database:Password" "taskly" --project src/Simplifyme.Taskly.Main  
```
(At least try to understand what you are doing here, search for the user secrets file on your operating system and see where it is stored.

Where in the code base is this user secret file **registered**? <br>How does the app know where to find the user secrets file?)

### Step 1: Persistence 💽
Implementing persistence for the **DOMAIN**
1. **TodoListRepository**: Implements the `ITodoListRepository` interface
    - Add a **DbSet<TodoList>** to the `DomainContextBase` class
    - Add a **TodoListConfiguration** class to **Config/Domain** and add it to the `OnModelCreating` method of the `DomainContextBase` class
      - Here is the mapping of **Tasks** in the **TodoList**. The rest of the properties you will need to implement yourself:
        ```csharp
        builder.HasMany(x => x.Tasks)
            .WithOne()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(x => x.Tasks)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
        ```
    - Add a **TaskConfiguration** class to **Config/Domain** and add it to the `OnModelCreating` method of the `DomainContextBase` class
    - Don't forget to register the TodoListRepository in the **Dependency Injection** container in **Main**

Implementing persistence for the **APPLICATION (Queries)**
1. **AllTodoListsQuery**: Implements the `IAllTodoListsQuery` interface
    - Don't forget to use the linq method **Include** in your query to include the **Tasks** and **User** in the result. Otherwise, the result will not contain the tasks and user data.
    - Add a **DbSet<TodoListData>** to the `QueryContextBase` class.
    - Add a **TodoListDataConfiguration** class to **Config/Data** and add it to the `OnModelCreating` method of the `QueryContextBase` class
      - Notice how TodoListData contains a full user data object and **TodoList** contains a userId. This is because the **TodoListData** is used for external presentation and should contain all the data needed for the frontend. This is okay because the **TodoListData** is not used in the domain layer and is not persisted in the database, it's readonly.
    - Don't forget to register the AllTodoListsQuery in the **Dependency Injection** container in **Main**

Because we are using a **CodeFirst**, we need to create a migration to create the database.
- Create a first migration for the **DomainContextPostgres**.
```bash
dotnet ef migrations add Initial \
-o Persistence/EntityFramework/Migrations/PostgreSQL \
--context DomainContextPostgres \
--project src/Simplifyme.Taskly.Infrastructure \
--startup-project src/Simplifyme.Taskly.Main \
--verbose
```
Each time you change the model or entity configuration classes, you need to create a new migration.
Therefore you can replace the **Initial** with a name of your choice.
E.g. **AddTask** or **AddTodoList**.

Each time the application starts, it will check if there is a migration that has not been applied yet. <br> If there is, it will apply the migration to the database.

Run the application to create the database and apply the migration.
  - Check the database to see if the tables are created correctly.

### Step 2: WebAPI 🌐
Implement the following controllers:
1. **CreateTodoListController**
   - Endpoint: `POST /api/todolists`
   - Output: ID of the created list
   - Actions: Trigger the use case to create a todo list
1. **AddTaskToTodoListController**
   - Endpoint: `POST /api/todolists/{todoListId}/tasks`
   - Output: location header /todolists/{todoListId}/tasks/{taskId}
   - Actions: Trigger the use case to add a task to a todo list
3. **AllTodoListsByUserIdController**
   - Endpoint: `GET /api/todolists/by-user/{userId}`
   - Actions: Trigger the use case to get all todo lists by user ID
4. **CompleteTaskController**
    - Endpoint: `POST /api/todolists/{todoListId}/tasks/{taskId}/complete`
    - Actions: Trigger the use case to complete a task
5. **TransferTaskController** (optional)
    - Endpoint: `POST /api/todolists/{sourceListId}/tasks/{taskId}/transfer`
    - Actions: Trigger the use case to transfer a task to another list

Don't forget the complete the **Routes.cs** file with the new routes.

Run the application and test the endpoints. 

**CreateTodoList** should also emitt a domain event. You can check this by checking the queues in LavinMQ.

### Step 3: Consuming messages 📨

In this step, you will consume the messages from the **message broker** and trigger the corresponding use cases.

Emitting domain events is already done in the **Domain** layer.

Implement the following controllers:
1. **SuspendUserController**
    - Triggered when: 'UserUnsubscribed' or 'PaymentError' event is received.
      - Look up the asyncapi.yaml file more details like the operationId and the payload.
    - Actions: Trigger the use case to suspend a user.

Test the endpoint by sending a message to the queue. In the database, the user should be suspended.
## Some useful commands 🛠️
Below are some useful commands, that you might need during the lab. It's a good idea to collect all the commands you need in these lessons in a single place.

### Run the application
```bash
dotnet run --project src/Simplifyme.Taskly.Main
```

### Run the tests
```bash
dotnet test
```

### Swagger UI
```
http://localhost:5273/swagger/index.html
```

### Create a new migration
```bash
 # Initial is the name of the migration, you can change it to whatever you want
dotnet ef migrations add Initial \
-o Persistence/EntityFramework/Migrations/PostgreSQL \
--context DomainContextPostgres \
--project src/Simplifyme.Taskly.Infrastructure \
--startup-project src/Simplifyme.Taskly.Main \
--verbose
```

### Startup docker containers
```bash
 docker compose -f config/docker-compose.yml up -d --remove-orphans
```

### Stop docker containers
```bash
 docker compose -f config/docker-compose.yml down
```



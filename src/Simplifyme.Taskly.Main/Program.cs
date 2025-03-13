using Simplifyme.Taskly.Main.Modules.Messaging;
using Simplifyme.Taskly.Main.Modules.Persistence;
using Simplifyme.Taskly.Main.Modules.WebApi;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder
    .Services
        .AddPersistenceModule(configuration)
        .AddWebApiModule(configuration)
        .AddMessagingModule(configuration);

var app = builder
    .Build()
    .UsePersistenceModule()
    .UseWebApiModule();

await app.RunMessagingModule();
await app.RunAsync();
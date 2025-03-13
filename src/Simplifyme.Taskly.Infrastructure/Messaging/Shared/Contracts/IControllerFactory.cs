namespace Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;

public interface IControllerFactory
{
    IController<ConsumerContext> CreateController(ConsumerContext consumerContext);
}

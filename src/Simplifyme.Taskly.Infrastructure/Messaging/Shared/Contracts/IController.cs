namespace Simplifyme.Taskly.Infrastructure.Messaging.Shared.Contracts;

public interface IController<in Context>
{
    Task Handle(Context context);
}

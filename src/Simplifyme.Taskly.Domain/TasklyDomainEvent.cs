using Domaincrafters.Domain;

namespace Simplifyme.Taskly.Domain.Shared;

public abstract class TasklyDomainEvent(string eventName) : BaseDomainEvent(eventName, "taskly");
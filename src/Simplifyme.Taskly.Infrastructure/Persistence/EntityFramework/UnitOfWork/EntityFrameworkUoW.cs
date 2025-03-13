using Domaincrafters.Application;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.UnitOfWork;

public class EntityFrameworkUoW(
    DomainContextBase domainContext,
    ILogger<EntityFrameworkUoW> logger
) : IUnitOfWork
{
    private readonly DbContext _domainContext = domainContext;
    private readonly ILogger<EntityFrameworkUoW> _logger = logger;

    public async Task Do(Func<Task>? action = null)
    {
        if (action is not null)
            _logger.LogWarning("Delegates passed to EF unit of work are ignored.");

        await _domainContext.SaveChangesAsync();
    }
}
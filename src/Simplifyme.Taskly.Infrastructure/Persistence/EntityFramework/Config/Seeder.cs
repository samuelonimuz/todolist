using Microsoft.EntityFrameworkCore;
using Simplifyme.Taskly.Domain.User;

namespace Simplifyme.Taskly.Infrastructure.Persistence.EntityFramework.Config;

public static class Seeder
{

    private static readonly IList<User> _users = [
        User.Create("Matthias", new UserId("eba0c4b2-1f3d-4e5b-8f6c-7a2d9e5f3b8a")),
        User.Create("Dimi", new UserId("f3b0c4b2-1f3d-4e5b-8f6c-7a2d9e5f3b8a")),
    ];

    public static void Seed(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasData(_users);
    }
}
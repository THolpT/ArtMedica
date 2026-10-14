using Microsoft.EntityFrameworkCore;

public sealed class AdminOptions<TDbContext>
    where TDbContext : DbContext
{
    public required AdminRegistry Registry { get; init; }
}
using ArtMedica.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace ArtMedica.Application;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public virtual DbSet<Project> Projects {get;set;}
    public virtual DbSet<Case> Cases {get;set;}
    public virtual DbSet<Service> Services {get;set;}
    public virtual DbSet<New> News {get;set;}
    public virtual DbSet<OperationResult> OperationResults {get;set;}
    public virtual DbSet<ProjectSlideInfo> ProjectSlideInfos {get;set;}
    public virtual DbSet<Promotion> Promotions {get;set;}
    public virtual DbSet<ServiceCategory> ServiceCategories {get;set;}
    public virtual DbSet<Specialist> Specialists {get;set;}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder.UseNpgsql("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
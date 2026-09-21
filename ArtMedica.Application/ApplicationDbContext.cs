using ArtMedica.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArtMedica.Application;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<Case> Cases { get; set; }
    public virtual DbSet<Service> Services { get; set; }
    public virtual DbSet<New> News { get; set; }
    public virtual DbSet<OperationResult> OperationResults { get; set; }
    public virtual DbSet<ProjectSlideInfo> ProjectSlideInfos { get; set; }
    public virtual DbSet<Promotion> Promotions { get; set; }
    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
    public virtual DbSet<Specialist> Specialists { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
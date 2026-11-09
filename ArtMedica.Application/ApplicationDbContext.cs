using System.Text.Json;
using ArtMedica.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ArtMedica.Application;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // Справочники / самостоятельные
    public virtual DbSet<Case> Cases { get; set; }
    public virtual DbSet<Promotion> Promotions { get; set; }
    public virtual DbSet<Faq> Faqs { get; set; }
    public virtual DbSet<MediaFile> MediaFiles { get; set; }
    public virtual DbSet<New> News { get; set; }

    // Документы
    public virtual DbSet<Document> Documents { get; set; }
    public virtual DbSet<DocumentType> DocumentTypes { get; set; }

    // Услуги
    public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
    public virtual DbSet<Service> Services { get; set; }
    public virtual DbSet<OperationResult> OperationResults { get; set; }

    // Проекты
    public virtual DbSet<Project> Projects { get; set; }
    public virtual DbSet<ProjectSlideInfo> ProjectSlideInfos { get; set; }
    public virtual DbSet<ProjectFormat> ProjectFormats { get; set; }
    public virtual DbSet<ProjectPartner> ProjectPartners { get; set; }

    // Специалисты
    public virtual DbSet<Specialist> Specialists { get; set; }
    public virtual DbSet<Qualification> Qualifications { get; set; }
    public virtual DbSet<Accreditation> Accreditations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureProjects(modelBuilder);
        ConfigureSpecialists(modelBuilder);
        ConfigureServices(modelBuilder);
        ConfigureOperationResults(modelBuilder);
        ConfigureDocuments(modelBuilder);
        ConfigureOtherEntities(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder builder)
    {
        builder.Properties<DateTime>()
            .HaveConversion<UtcDateTimeConverter>();
    }

    // ========================================================================
    //  ПРОЕКТЫ
    // ========================================================================
    private static void ConfigureProjects(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(builder =>
        {
            builder.ToTable("Projects");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.FirstDescription).HasMaxLength(2000);
            builder.Property(x => x.SecondDescription).HasMaxLength(2000);

            builder.PrimitiveCollection(x => x.Stages)
                .HasColumnName("Stages")
                .HasColumnType("text[]");

            builder.HasOne(x => x.MainImgUrl)
                   .WithMany()
                   .HasForeignKey(x => x.MainImgUrlId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.SlideInfo)
                   .WithOne(s => s.Project)
                   .HasForeignKey<ProjectSlideInfo>(s => s.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Formats)
                   .WithOne(f => f.Project)
                   .HasForeignKey(f => f.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Partners)
                   .WithOne(p => p.Project)
                   .HasForeignKey(p => p.ProjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Images)
                   .WithMany()
                   .UsingEntity<Dictionary<string, object>>(
                       "ProjectImages",
                       j => j.HasOne<MediaFile>()
                             .WithMany()
                             .HasForeignKey("FileId")
                             .OnDelete(DeleteBehavior.Cascade),
                       j => j.HasOne<Project>()
                             .WithMany()
                             .HasForeignKey("ProjectId")
                             .OnDelete(DeleteBehavior.Cascade),
                       j =>
                       {
                           j.ToTable("ProjectImages");
                           j.HasKey("ProjectId", "FileId");
                       });
        });

        modelBuilder.Entity<ProjectSlideInfo>(builder =>
        {
            builder.ToTable("ProjectSlideInfos");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.Date).IsRequired();

            builder.HasOne(x => x.ImgUrl)
                   .WithMany()
                   .HasForeignKey(x => x.ImgUrlId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProjectFormat>(builder =>
        {
            builder.ToTable("ProjectFormats");
            builder.HasKey(x => x.Id);

            builder.HasOne(f => f.Img)
                   .WithMany()
                   .HasForeignKey(f => f.ImgId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProjectPartner>(builder =>
        {
            builder.ToTable("ProjectPartners");
            builder.HasKey(x => x.Id);
        });
    }

    // ========================================================================
    //  СПЕЦИАЛИСТЫ
    // ========================================================================
    private static void ConfigureSpecialists(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Specialist>(builder =>
        {
            builder.ToTable("Specialists");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.MidName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Department).HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(2000);

            builder.PrimitiveCollection(x => x.ActivityArea).HasColumnName("ActivityArea");

            builder.HasOne(x => x.Img)
                   .WithMany()
                   .HasForeignKey(x => x.ImgId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Video)
                   .WithMany()
                   .HasForeignKey(x => x.VideoId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Qualifications)
                   .WithOne(q => q.Specialist)
                   .HasForeignKey(q => q.SpecialistId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Accreditations)
                   .WithOne(a => a.Specialist)
                   .HasForeignKey(a => a.SpecialistId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.operationResults)
                   .WithOne(r => r.Specialist)
                   .HasForeignKey(r => r.SpecialistId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.LastName);
        });

        modelBuilder.Entity<Qualification>(builder => builder.ToTable("Qualifications"));
        modelBuilder.Entity<Accreditation>(builder => builder.ToTable("Accreditations"));
    }

    // ========================================================================
    //  УСЛУГИ
    // ========================================================================
    private static void ConfigureServices(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceCategory>(builder =>
        {
            builder.ToTable("ServiceCategories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);

            builder.HasOne(x => x.ImgUrl)
                   .WithMany()
                   .HasForeignKey(x => x.ImgUrlId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Services)
                   .WithOne(s => s.Category)
                   .HasForeignKey(s => s.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Service>(builder =>
        {
            builder.ToTable("Services");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).HasColumnType("decimal(18,2)");
            builder.Property(x => x.NomenclatureCode).IsRequired().HasMaxLength(50);
            builder.Property(x => x.MISCode).IsRequired().HasMaxLength(50);
            builder.Property(x => x.DisplayTitle).IsRequired().HasMaxLength(200);
            builder.Property(x => x.TechnicalTitle).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.Preparation).HasMaxLength(2000);
            builder.Property(x => x.Types).HasMaxLength(2000);
        });
    }

    // ========================================================================
    //  РЕЗУЛЬТАТЫ ОПЕРАЦИЙ
    // ========================================================================
    private static void ConfigureOperationResults(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OperationResult>(builder =>
        {
            builder.ToTable("OperationResults");
            builder.HasKey(x => x.Id);

            builder.HasOne(x => x.ImgUrl)
                   .WithMany()
                   .HasForeignKey(x => x.ImgUrlId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Operation)
                   .WithMany()
                   .HasForeignKey(x => x.OperationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Specialist)
                   .WithMany(s => s.operationResults)
                   .HasForeignKey(x => x.SpecialistId)
                   .OnDelete(DeleteBehavior.Restrict);
        });
    }

    // ========================================================================
    //  ДОКУМЕНТЫ
    // ========================================================================
    private static void ConfigureDocuments(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<DocumentType>(builder =>
        {
            builder.ToTable("DocumentTypes");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(500);
        });

        modelBuilder.Entity<Document>(builder =>
        {
            builder.ToTable("Documents");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Url).IsRequired().HasMaxLength(500);

            builder.HasOne(x => x.Type)
                   .WithMany()
                   .HasForeignKey(x => x.TypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.TypeId);
        });
    }

    // ========================================================================
    //  ПРОЧЕЕ
    // ========================================================================
    private static void ConfigureOtherEntities(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<New>(builder =>
        {
            builder.ToTable("News");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Title).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).HasMaxLength(2000);

            builder.HasOne(x => x.ImgUrl)
                   .WithMany()
                   .HasForeignKey(x => x.ImgUrlId)
                   .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Case>(builder => builder.ToTable("Cases"));
        modelBuilder.Entity<Promotion>(builder => builder.ToTable("Promotions"));
        modelBuilder.Entity<Faq>(builder => builder.ToTable("Faqs"));
        modelBuilder.Entity<MediaFile>(builder => builder.ToTable("Files"));
    }

    // ========================================================================
    //  UTC converter
    // ========================================================================
    public class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
    {
        public UtcDateTimeConverter()
            : base(v => v.Kind == DateTimeKind.Unspecified
                        ? DateTime.SpecifyKind(v, DateTimeKind.Utc)
                        : v.ToUniversalTime(),
                   v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
        { }
    }
}
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using UseCaseManagement.Application.Common;
using UseCaseManagement.Domain.Entities;

namespace UseCaseManagement.Infrastructure;

public class UseCaseDbContext :  DbContext, IUnitOfWork
{
    public DbSet<LogSource> LogSource {  get; set; } 
    public DbSet<LogSourceFile> LogSourceFile {  get; set; } 
    public DbSet<UseCase> UseCase {  get; set; } 
    public DbSet<UseCaseFile> UseCaseFile {  get; set; }
    public DbSet<Vendor> Vendor { get; set; }

    public UseCaseDbContext() { }
    
    public UseCaseDbContext(DbContextOptions<UseCaseDbContext> options): base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseNpgsql(@"Server=localhost;Port=5432;Database=UseCase;User Id=postgres;Password=hugo;");
        }
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        modelBuilder.Entity<LogSource>()
            .HasMany(e => e.UseCases)
            .WithMany(e => e.LogSources)
            .UsingEntity(j => j.ToTable("UseCaseLogSources"));
        
        modelBuilder.Entity<UseCase>()
            .HasMany(e => e.Vendors)
            .WithMany(e => e.UseCases)
            .UsingEntity(j => j.ToTable("VendorUseCase"));

        base.OnModelCreating(modelBuilder);
    }
}

using System.Linq.Expressions;
using Aguacongas.AspNetCore.Authentication.EntityFramework;
using CodeSecure.Core.Entity;
using CodeSecure.Core.EntityFramework;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EnvironmentName = CodeSecure.Core.Entity.EnvironmentName;

namespace CodeSecure.Application;

public class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<Users, Roles, Guid, UserClaims, UserRoles, UserLogins, RoleClaims, UserTokens>(options)
{
    public DbSet<AuthProviders> AuthProviders { get; set; } = null!;
    public DbSet<AppSettings> AppSettings { set; get; } = null!;
    public DbSet<CiTokens> CiTokens { set; get; } = null!;
    public DbSet<ContainerPackages> ContainerPackages { set; get; } = null!;
    public DbSet<Containers> Containers { set; get; } = null!;
    public DbSet<EnvironmentName> EnvironmentName { set; get; } = null!;
    public DbSet<FindingActivities> FindingActivities { set; get; } = null!;
    public DbSet<Findings> Findings { set; get; } = null!;
    public DbSet<PackageDependencies> PackageDependencies { set; get; } = null!;
    public DbSet<Packages> Packages { set; get; } = null!;
    public DbSet<PackageVulnerabilities> PackageVulnerabilities { set; get; } = null!;
    public DbSet<GitCommits> ProjectCommits { set; get; } = null!;
    public DbSet<ProjectContainers> ProjectContainers { set; get; } = null!;
    public DbSet<ProjectEnvironmentVariables> ProjectEnvironmentVariables { set; get; } = null!;
    public DbSet<ProjectSettings> ProjectConfig { set; get; } = null!;
    public DbSet<ProjectPackages> ProjectPackages { set; get; } = null!;
    public DbSet<Projects> Projects { set; get; } = null!;
    public DbSet<ProjectSettings> ProjectSettings { set; get; } = null!;
    public DbSet<ProjectTags> ProjectTags { set; get; } = null!;
    public DbSet<ProjectUsers> ProjectUsers { set; get; } = null!;
    public DbSet<Rules> Rules { set; get; } = null!;
    public DbSet<ScanFindings> ScanFindings { set; get; } = null!;
    public DbSet<Scanners> Scanners { set; get; } = null!;
    public DbSet<ScanProjectPackages> ScanProjectPackages { set; get; } = null!;
    public DbSet<Scans> Scans { set; get; } = null!;
    public DbSet<SourceControls> SourceControls { set; get; } = null!;
    public DbSet<Tags> Tags { set; get; } = null!;
    public DbSet<Tickets> Tickets { set; get; } = null!;
    public DbSet<Vulnerabilities> Vulnerabilities { set; get; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SchemeDefinition>(b =>
        {
            b.ToTable(nameof(AuthProviders));
            b.Ignore(((Expression<Func<SchemeDefinition, object>>)(p => p.Options))!)
                .Ignore(((Expression<Func<SchemeDefinition, object>>)(p => p.HandlerType))!)
                .HasKey(((Expression<Func<SchemeDefinition, object>>)(p => p.Scheme))!);
            b.Property<string>(p => p.ConcurrencyStamp).IsConcurrencyToken();
        });
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Users>(b => { b.ToTable(nameof(Core.Entity.Users)); });
        modelBuilder.Entity<Roles>(b => { b.ToTable(nameof(Core.Entity.Roles)); });
        modelBuilder.Entity<RoleClaims>(b => { b.ToTable(nameof(Core.Entity.RoleClaims)); });
        modelBuilder.Entity<UserClaims>(b => { b.ToTable(nameof(Core.Entity.UserClaims)); });
        modelBuilder.Entity<UserLogins>(b => { b.ToTable(nameof(Core.Entity.UserLogins)); });
        modelBuilder.Entity<UserRoles>(b => { b.ToTable(nameof(Core.Entity.UserRoles)); });
        modelBuilder.Entity<UserTokens>(b => { b.ToTable(nameof(Core.Entity.UserTokens)); });
    }

    public override int SaveChanges()
    {
        UpdateTime();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
    {
        UpdateTime();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateTime()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e is { Entity: BaseEntity, State: EntityState.Added or EntityState.Modified });
        foreach (var entity in entries)
        {
            ((BaseEntity)entity.Entity).UpdatedAt = DateTime.UtcNow;
            if (entity.State == EntityState.Added) ((BaseEntity)entity.Entity).CreatedAt = DateTime.UtcNow;
        }
    }
}
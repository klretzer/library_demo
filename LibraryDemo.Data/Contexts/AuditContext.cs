namespace LibraryDemo.Data.Contexts;

public sealed class AuditContext(IOptions<ConnectionStrings> strings) : DbContext
{
    public DbSet<SaveChangesAudit>? SaveChangesAudits { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlServer(strings.Value["Library"], sqlOptions =>
        {
            sqlOptions.EnableRetryOnFailure();
            sqlOptions.MigrationsHistoryTable("__MigrationsHistory", "Audit");
        });
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Audit");
        modelBuilder.Entity<User>().ToTable("Users", "Identity", t => t.ExcludeFromMigrations());

        base.OnModelCreating(modelBuilder);
    }
}
namespace LibraryDemo.Data.Contexts;

public class IdentityContext(DbContextOptions<IdentityContext> options) : IdentityDbContext<User, IdentityRole<Guid>, Guid>(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Identity");

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(e => e.ToTable("Users"));
        modelBuilder.Entity<IdentityRole<Guid>>(e => e.ToTable("Roles"));
        modelBuilder.Entity<IdentityUserClaim<Guid>>(e => e.ToTable("UserClaims"));
        modelBuilder.Entity<IdentityUserRole<Guid>>(e => e.ToTable("UserRoles"));
        modelBuilder.Entity<IdentityUserLogin<Guid>>(e => e.ToTable("UserLogins"));
        modelBuilder.Entity<IdentityUserToken<Guid>>(e => e.ToTable("UserTokens"));
        modelBuilder.Entity<IdentityRoleClaim<Guid>>(e => e.ToTable("RoleClaims"));
    }
}
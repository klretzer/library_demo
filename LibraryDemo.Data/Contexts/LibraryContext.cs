namespace LibraryDemo.Data.Contexts;

public sealed class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public required DbSet<Book> Books { get; set; }

    public required DbSet<BookReview> BookReviews { get; set; }

    public required DbSet<BookRental> BookRentals { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("Domain");
        modelBuilder.Entity<User>().ToTable("Users", "Identity", t => t.ExcludeFromMigrations());

        base.OnModelCreating(modelBuilder);
    }
}
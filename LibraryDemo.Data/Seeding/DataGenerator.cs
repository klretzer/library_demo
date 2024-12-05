namespace LibraryDemo.Data.Seeding;

public sealed class DataGenerator(IOptions<LibrarySettings> config)
{
    private readonly SeedSettings _config = config.Value.Seed!;

    private readonly IPasswordHasher<User> _passwordHasher = new PasswordHasher<User>();

    private static Faker<Book> GetBookFaker()
    {
        return new Faker<Book>()
            .RuleFor(b => b.Id, _ => Guid.NewGuid())
            .RuleFor(b => b.Category, f => f.Commerce.Department())
            .RuleFor(b => b.CoverImageUrl, f => f.Image.LoremFlickrUrl())
            .RuleFor(b => b.ISBN, _ => Guid.NewGuid())
            .RuleFor(b => b.Description, f => f.Commerce.ProductDescription())
            .RuleFor(b => b.PageCount, f => f.Random.Int(50, 10000))
            .RuleFor(b => b.PublicationDate, f => DateOnly.FromDateTime(f.Date.Recent()))
            .RuleFor(b => b.Title, f => f.Hacker.Phrase())
            .RuleFor(b => b.Publisher, f => f.Company.CompanyName())
            .RuleFor(b => b.Author, f => f.Name.FullName());
    }

    private static Faker<BookReview> GetReviewFaker()
    {
        return new Faker<BookReview>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.Rating, f => f.Random.Number(1, 5))
            .RuleFor(r => r.Text, f => f.Rant.Review());
    }

    private static Faker<BookRental> GetRentalFaker()
    {
        return new Faker<BookRental>()
            .RuleFor(r => r.Id, _ => Guid.NewGuid())
            .RuleFor(r => r.DueDate, f => f.Date.Between(DateTime.Now.AddDays(-15), DateTime.Now.AddDays(5)));
    }

    private Faker<User> GetUserFaker()
    {
        return new Faker<User>()
            .RuleFor(u => u.Id, (_, u) => Guid.NewGuid())
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.PasswordHash, (_, u) => _passwordHasher.HashPassword(u, _config.DefaultPassword));
    }

    public List<Book> GetFakeBooks()
    {
        var numBooks = _config.NumberOfFakeBooks;

        return GetBookFaker().Generate(numBooks);
    }

    public List<BookReview> GetFakeReviews(IEnumerable<User> seedUsers, IEnumerable<Book> seedBooks)
    {
        var fakeReviews = GetReviewFaker().Generate(_config.NumberOfFakeReviews);
        var randomGen = new Random();
        var userIndexMax = seedUsers.Count() - 1;
        var bookIndexMax = seedBooks.Count() - 1;

        foreach (var review in fakeReviews)
        {
            var userIndex = randomGen.Next(0, userIndexMax);
            var bookIndex = randomGen.Next(0, bookIndexMax);

            review.UserId = seedUsers.ElementAt(userIndex).Id;
            review.BookId = seedBooks.ElementAt(bookIndex).Id;
        }

        return fakeReviews;
    }

    public List<BookRental> GetFakeRentals(IEnumerable<User> seedUsers, IEnumerable<Book> seedBooks)
    {
        var fakeRentals = GetRentalFaker().Generate(_config.NumberOfFakeRentals);
        var randomGen = new Random();
        var userIndexMax = seedUsers.Count() - 1;
        var bookIndexMax = seedBooks.Count() - 1;

        foreach (var rental in fakeRentals)
        {
            var userIndex = randomGen.Next(0, userIndexMax);
            var bookIndex = randomGen.Next(0, bookIndexMax);

            rental.UserId = seedUsers.ElementAt(userIndex).Id;
            rental.BookId = seedBooks.ElementAt(bookIndex).Id;
        }

        return fakeRentals;
    }

    public List<User> GetFakeUsers()
    {
        var numUsers = _config.NumberOfFakeUsers;
        var myUsers = new List<User> {
            new() { Id = Guid.NewGuid(), UserName = "kyle_admin", Email = "kyle_admin@testlibrary.com" },
            new() { Id = Guid.NewGuid(), UserName = "kyle_librarian", Email = "kyle_librarian@testlibrary.com" },
            new() { Id = Guid.NewGuid(), UserName = "kyle_customer", Email = "kyle_customer@testlibrary.com" }
        };

        foreach (var user in myUsers)
        {
            user.PasswordHash = _passwordHasher.HashPassword(user, _config.DefaultPassword);
        }

        List<User> fakeUsers = [.. myUsers, .. GetUserFaker().Generate(numUsers)];

        return fakeUsers;
    }

    public List<IdentityUserRole<Guid>> GetFakeUserRoles(IEnumerable<User> users, IEnumerable<IdentityRole<Guid>> roles)
    {
        var myAdmin = users.FirstOrDefault(u => u.UserName!.Equals("kyle_admin"));
        var myLibrarian = users.FirstOrDefault(u => u.UserName!.Equals("kyle_librarian"));
        var myCustomer = users.FirstOrDefault(u => u.UserName!.Equals("kyle_customer"));
        var myGuidList = new List<Guid> { myAdmin!.Id, myLibrarian!.Id, myCustomer!.Id };

        List<IdentityUserRole<Guid>> userRoles = [
            new IdentityUserRole<Guid> { UserId = myAdmin!.Id, RoleId = roles.Single(r => r.Name == "Admin").Id },
            new IdentityUserRole<Guid> { UserId = myLibrarian!.Id, RoleId = roles.Single(r => r.Name == "Librarian").Id },
            new IdentityUserRole<Guid> { UserId = myCustomer!.Id, RoleId = roles.Single(r => r.Name == "Customer").Id }
        ];

        foreach (var user in users)
        {
            if (myGuidList.Contains(user.Id)) continue;

            userRoles.Add(new IdentityUserRole<Guid> { UserId = user.Id, RoleId = roles.Single(r => r.Name == "Customer").Id });
        }

        return userRoles;
    }
}
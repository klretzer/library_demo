namespace LibraryDemo.Core.Mapping;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<Book, BookDTO>();

        CreateMap<BookRental, BookRentalDTO>();

        CreateMap<BookReview, BookReviewDTO>();
    }
}
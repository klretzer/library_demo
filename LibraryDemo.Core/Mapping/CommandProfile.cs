namespace LibraryDemo.Core.Mapping;

public class CommandProfile : Profile
{
    public CommandProfile()
    {
        CreateMap<CreateBookCommand, Book>();
        CreateMap<UpdateBookCommand, Book>();

        CreateMap<CreateBookRentalCommand, BookRental>();
        CreateMap<UpdateBookRentalCommand, BookRental>();

        CreateMap<CreateBookReviewCommand, BookReview>();
        CreateMap<UpdateBookReviewCommand, BookReview>();
    }
}
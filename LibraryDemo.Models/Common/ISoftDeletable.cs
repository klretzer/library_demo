namespace LibraryDemo.Models.Common;

public interface ISoftDeletable
{
    public bool IsDeleted { get; set; }
}
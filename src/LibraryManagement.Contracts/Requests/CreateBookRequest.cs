namespace LibraryManagement.Contracts.Requests
{
    public class CreateBookRequest
    {
        public required string Title { get; init; }
        public required string Author { get; init; }
        public required string ISBN { get; init; }
    }
}

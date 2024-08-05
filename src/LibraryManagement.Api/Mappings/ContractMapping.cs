using LibraryManagement.Application.Models;
using LibraryManagement.Contracts.Requests;
using LibraryManagement.Contracts.Responses;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace LibraryManagement.Api.Mappings
{
    //I have used AutoMapper in previous projects to handle mapping between objects but I find for projects like this it is often easier to just implement your own mapping without the use of a third-party library
    public static class ContractMapping
    {
        //Book Mappings
        public static Book MapToBook(this CreateBookRequest request)
        {
            return new Book
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                IsAvailable = true
            };
        }

        public static BookResponse MapToResponse(this Book book)
        {
            return new BookResponse
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                ISBN = book.ISBN,
                IsAvailable = book.IsAvailable
            };
        }

        public static BooksResponse MapToResponse(this IEnumerable<Book> books)
        {
            return new BooksResponse
            {
                Items = books.Select(MapToResponse)
            };
        }

        public static Book MapToBook(this UpdateBookRequest request, Guid id) 
        {
            return new Book
            {
                Id = id,
                Title = request.Title,
                Author = request.Author,
                ISBN = request.ISBN,
                IsAvailable = request.IsAvailable
            };
        }

        //User Mappings
        public static User MapToUser(this CreateUserRequest request)
        {
            return new User
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Books = []
            };
        }

        public static UserResponse MapToResponse(this User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Books = user.Books.Select(MapToResponse).ToList()
            };
        }

        public static User MapToUser(this UpdateUserRequest request, Guid id)
        {
            return new User
            {
                Id = id,
                Name = request.Name,
                Books = []
            };
        }

        //Result Mapping
        public static ResultResponse<UserResponse> MapToResponse(this Result<User> result)
        {
            return new ResultResponse<UserResponse>
            {
                Response = result.Model?.MapToResponse(),
                Success = result.Success,
                Error = result.Error
            };
        }
    }
}

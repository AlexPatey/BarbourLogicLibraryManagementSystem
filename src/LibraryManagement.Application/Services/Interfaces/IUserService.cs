using LibraryManagement.Application.Models;

namespace LibraryManagement.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> CreateAsync(User user);
        Task<User?> GetAsync(Guid id);
        Task<bool> UpdateAsync(User user);
        Task<bool> DeleteByIdAsync(Guid id);
        Task<Result<User>> LendBookAsync(Guid userId, Guid bookId);
        Task<Result<User>> ReturnBookAsync(Guid userId, Guid bookId);
    }
}

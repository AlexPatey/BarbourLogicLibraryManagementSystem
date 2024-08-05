using LibraryManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<bool> CreateAsync(User user);
        Task<User?> GetAsync(Guid id);
        Task<bool> UpdateAsync(User user);
        Task<bool> UpdateUsersBooksAsync(User user);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}

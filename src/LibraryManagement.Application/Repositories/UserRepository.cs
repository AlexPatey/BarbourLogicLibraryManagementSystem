using LibraryManagement.Application.Models;
using LibraryManagement.Application.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Repositories
{
    public class UserRepository : IUserRepository
    {
        /*I am using a private readonly list to represent a User database table, in a real scenario I would likely use EF Core, if it is known beforehand that a project is going to have very simple logic,
        then a light-weight ORM such as Dapper might be more appropriate. Please note I have a WritingsAPI project on my GitHub if you would like to see an example of how I use EF Core in my Web APIs. */
        private readonly List<User> _users = [];
        
        public async Task<bool> CreateAsync(User user)
        {
            /*If I was using EF Core here, then I would check that the result of <DbContext>.SaveChangesAsync() is greater than 0 to determine whether I should return true or false (if the result of SaveChangesAsync() 
            is 0, then that means no records were updated, and that you should return false). Returning true here because I am just using a list to represent my Db table.*/
            _users.Add(user);
            return true;
        }

        public async Task<User?> GetAsync(Guid id)
        {
            return _users.SingleOrDefault(u => u.Id == id);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var userIndex = _users.FindIndex(u => u.Id == user.Id);

            if (userIndex is -1)
            {
                return false;
            }

            _users[userIndex].Name = user.Name;

            return true;
        }

        public async Task<bool> UpdateUsersBooksAsync(User user)
        {
            var userIndex = _users.FindIndex(u => u.Id == user.Id);

            if (userIndex is -1)
            {
                return false;
            }

            _users[userIndex].Books = user.Books;

            return true;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = _users.RemoveAll(u => u.Id == id);
            return removedCount > 0;
        }
    }
}

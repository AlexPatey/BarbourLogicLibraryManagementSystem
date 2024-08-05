using FluentValidation;
using LibraryManagement.Application.Models;
using LibraryManagement.Application.Repositories;
using LibraryManagement.Application.Repositories.Interfaces;
using LibraryManagement.Application.Services.Interfaces;
using LibraryManagement.Application.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services
{
    public class UserService(IUserRepository userRepository, IBookRepository bookRepository, IValidator<User> userValidator) : IUserService
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IBookRepository _bookRepository = bookRepository;
        private readonly IValidator<User> _userValidator = userValidator;

        public async Task<bool> CreateAsync(User user)
        {
            try
            {
                await _userValidator.ValidateAndThrowAsync(user);
                return await _userRepository.CreateAsync(user);
            }
            catch (Exception ex)
            {
                //In a real scenario you would want to add some logging here, using something like Serilog. Please note I have a WritingsAPI on my GitHub if you would like to see an example of how I log, etc.
                throw;
            }
        }

        public async Task<User?> GetAsync(Guid id)
        {
            return await _userRepository.GetAsync(id);
        }

        public async Task<bool> UpdateAsync(User user)
        {
            try
            {
                await _userValidator.ValidateAndThrowAsync(user);
                return await _userRepository.UpdateAsync(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            //When deleting a user, you will need to ensure that all books associated with the user are returned
            var user = await _userRepository.GetAsync(id);

            if (user is null)
            {
                return false;
            }

            foreach (var book in user.Books)
            {
                book.IsAvailable = true;
                _ = await _bookRepository.UpdateAsync(book); /*In a real scenario we would want the return value of this method to be Task<<Result<bool>> in a similar manner as the below methods,
                                                             and if a book failed to update here, we would need to return an appropriate error message as done below*/
            }

            return await _userRepository.DeleteByIdAsync(id);
        }

        public async Task<Result<User>> LendBookAsync(Guid userId, Guid bookId)
        {
            //Ensure user exists
            var user = await _userRepository.GetAsync(userId);

            if (user is null)
            {
                return new Result<User> 
                { 
                    Model = null,
                    Success = false,
                    Error = $"User with Id {userId} could not be found." /*In a real scenario I would probably move these errors to a static class to avoid the use of "magic strings," 
                                                                         as these errors might be reused in other parts of the code; these same error strings could also be used in logs*/
                };
            }

            //Ensure book exists and is not unavailable
            var book = await _bookRepository.GetAsync(bookId);

            if (book is null)
            {
                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"Book with Id {bookId} could not be found."
                };
            }

            if (!book.IsAvailable)
            {
                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"Book with Id {bookId} is not available to lend."
                };
            }

            //Add book to user's list of books
            user.Books.Add(book);

            var isUserUpdated = await _userRepository.UpdateUsersBooksAsync(user);

            if (!isUserUpdated)
            {
                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"Could not update user with Id {userId}."
                };
            }

            //Mark book as unavailable
            book.IsAvailable = false;
            var isBookUpdated = await _bookRepository.UpdateAsync(book);

            if (!isBookUpdated)
            {
                /*In a real scenario if this step failed we would need to remove the book from the above user object, omitting this here because I am simply using lists to represent Db tables,
                we could use a C# library like Polly to retry each step if it failed*/

                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"Could not update book with Id {bookId}."
                };
            }

            return new Result<User>
            {
                Model = user,
                Success = true,
                Error = string.Empty
            };
        }

        public async Task<Result<User>> ReturnBookAsync(Guid userId, Guid bookId)
        {
            //Ensure user exists and has the book in question
            var user = await _userRepository.GetAsync(userId);

            if (user is null)
            {
                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"User with Id {userId} could not be found." /*In a real scenario I would probably move these errors to a static class to avoid the use of "magic strings," 
                                                                         as these errors might be reused in other parts of the code; these same error strings could also be used in logs*/
                };
            }

            if (user.Books.FindIndex(b => b.Id == bookId) is -1)
            {
                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"User with Id {userId} does not have book with Id {bookId}."
                };
            }

            //Ensure book exists
            var book = await _bookRepository.GetAsync(bookId);

            if (book is null)
            {
                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"Book with Id {bookId} could not be found."
                };
            }

            //Remove book from user's list of books
            user.Books.Remove(book);

            var isUserUpdated = await _userRepository.UpdateUsersBooksAsync(user);

            if (!isUserUpdated)
            {
                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"Could not update user with Id {userId}."
                };
            }

            //Mark book as available
            book.IsAvailable = true;
            var isBookUpdated = await _bookRepository.UpdateAsync(book);

            if (!isBookUpdated)
            {
                /*In a real scenario if this step failed we would need to add the book back to above user object, omitting this here because I am simply using lists to represent Db tables,
                we could use a C# library like Polly to retry each step if it failed*/

                return new Result<User>
                {
                    Model = null,
                    Success = false,
                    Error = $"Could not update book with Id {bookId}."
                };
            }

            return new Result<User>
            {
                Model = user,
                Success = true,
                Error = string.Empty
            };
        }
    }
}

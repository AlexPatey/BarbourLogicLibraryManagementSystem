using FluentValidation;
using LibraryManagement.Application.Models;
using LibraryManagement.Application.Repositories.Interfaces;
using LibraryManagement.Application.Services.Interfaces;

namespace LibraryManagement.Application.Services
{
    public class BookService(IBookRepository bookRepository, IValidator<Book> bookValidator) : IBookService
    {
		private readonly IBookRepository _bookRepository = bookRepository;
		private readonly IValidator<Book> _bookValidator = bookValidator;

        public async Task<bool> CreateAsync(Book book)
        {
			try
			{
				await _bookValidator.ValidateAndThrowAsync(book);
				return await _bookRepository.CreateAsync(book);
			}
			catch (Exception)
			{
				//In a real scenario you would want to add some logging here, using something like Serilog. Please note I have a WritingsAPI on my GitHub if you would like to see an example of how I log, etc.
				throw;
			}
        }

		public async Task<Book?> GetAsync(Guid id)
		{
			return await _bookRepository.GetAsync(id);
		}

        public async Task<List<Book>> GetAllAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<bool> UpdateAsync(Book book)
        {
			try
			{
				await _bookValidator.ValidateAndThrowAsync(book);
				return await _bookRepository.UpdateAsync(book);
			}
			catch (Exception)
			{
				throw;
			}
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
			return await _bookRepository.DeleteByIdAsync(id);
        }
    }
}

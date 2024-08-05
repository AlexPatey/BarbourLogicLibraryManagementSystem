using LibraryManagement.Application.Models;
using LibraryManagement.Application.Repositories.Interfaces;

namespace LibraryManagement.Application.Repositories
{
    public class BookRepository : IBookRepository
    {
        /*I am using a private readonly list to represent a Book database table, in a real scenario I would likely use EF Core, if it is known beforehand that a project is going to have very simple logic,
        then a light-weight ORM such as Dapper might be more appropriate. Please note I have a WritingsAPI project on my GitHub if you would like to see an example of how I use EF Core in my Web APIs. */
        private readonly List<Book> _books = [];

        public async Task<bool> CreateAsync(Book book)
        {
            /*If I was using EF Core here, then I would check that the result of <DbContext>.SaveChangesAsync() is greater than 0 to determine whether I should return true or false (if the result of SaveChangesAsync() 
            is 0, then that means no records were updated, and that you should return false). Returning true here because I am just using a list to represent my Db table.*/
            _books.Add(book);
            return true;
        }

        public async Task<Book?> GetAsync(Guid id)
        {
            return _books.SingleOrDefault(b => b.Id == id);
        }

        public async Task<List<Book>> GetAllAsync()
        {
            return _books;
        }

        public async Task<bool> UpdateAsync(Book book)
        {
            var bookIndex = _books.FindIndex(b => b.Id == book.Id);

            if (bookIndex is -1)
            {
                return false;
            }

            _books[bookIndex] = book;

            return true;
        }

        public async Task<bool> DeleteByIdAsync(Guid id)
        {
            var removedCount = _books.RemoveAll(b => b.Id == id);
            return removedCount > 0;
        }
    }
}

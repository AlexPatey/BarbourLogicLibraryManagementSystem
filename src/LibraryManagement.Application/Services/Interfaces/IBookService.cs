using LibraryManagement.Application.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Services.Interfaces
{
    public interface IBookService
    {
        Task<bool> CreateAsync(Book book);
        Task<Book?> GetAsync(Guid id);
        Task<List<Book>> GetAllAsync();
        Task<bool> UpdateAsync(Book book);
        Task<bool> DeleteByIdAsync(Guid id);
    }
}

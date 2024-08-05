using LibraryManagement.Application.Models;
using LibraryManagement.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Requests
{
    public class UpdateUserRequest
    {
        public required string Name { get; set; }
        public required IEnumerable<BookResponse> Books { get; set; } = [];
    }
}

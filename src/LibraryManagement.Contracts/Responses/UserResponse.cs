using LibraryManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Responses
{
    public class UserResponse
    {
        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public required List<BookResponse> Books { get; set; } = [];
    }
}

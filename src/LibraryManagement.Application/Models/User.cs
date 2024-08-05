using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Models
{
    public class User
    {
        public required Guid Id { get; init; }
        public required string Name { get; set; }
        public required List<Book> Books { get; set; } = [];
    }
}

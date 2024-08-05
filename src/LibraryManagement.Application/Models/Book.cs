using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Models
{
    public class Book
    {
        public required Guid Id { get; init; }
        public required string Title { get; set; }
        public required string Author { get; set; }
        public required string ISBN { get; set; }
        public required bool IsAvailable { get; set; }
    }
}

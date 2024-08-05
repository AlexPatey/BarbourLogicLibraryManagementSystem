using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Requests
{
    public class UpdateBookRequest
    {
        public required string Title { get; init; }
        public required string Author { get; init; }
        public required string ISBN { get; init; }
        public required bool IsAvailable { get; init; }
    }
}

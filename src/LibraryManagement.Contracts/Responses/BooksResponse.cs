using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Responses
{
    public class BooksResponse
    {
        public required IEnumerable<BookResponse> Items { get; init; } = Enumerable.Empty<BookResponse>();
    }
}

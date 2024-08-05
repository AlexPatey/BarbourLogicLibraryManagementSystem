using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Responses
{
    public class ResultResponse<TResponse>
    {
        public required TResponse? Response { get; set; }
        public required bool Success { get; set; }
        public required string Error { get; set; }
    }
}

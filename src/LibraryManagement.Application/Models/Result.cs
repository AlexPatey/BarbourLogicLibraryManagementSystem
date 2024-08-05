using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Models
{
    public class Result<TModel>
    {
        public required TModel? Model { get; set; }
        public required bool Success { get; set; }
        public required string Error { get; set; }
    }
}

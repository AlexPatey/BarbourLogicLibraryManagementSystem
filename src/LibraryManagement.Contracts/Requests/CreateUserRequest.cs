using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Contracts.Requests
{
    public class CreateUserRequest
    {
        public required string Name { get; set; }
    }
}

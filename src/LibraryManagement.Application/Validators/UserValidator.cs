using FluentValidation;
using LibraryManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Validators
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator() 
        {
            RuleFor(w => w.Id)
                .NotEmpty();

            RuleFor(w => w.Name)
                .NotEmpty()
                .MaximumLength(255); //In a real scenario you would probably want some limitation on how long a user's name should be, putting 255 here as a placeholder
        }
    }
}

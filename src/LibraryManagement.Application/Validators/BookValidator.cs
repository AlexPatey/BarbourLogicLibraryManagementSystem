using FluentValidation;
using LibraryManagement.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Validators
{
    public class BookValidator : AbstractValidator<Book>
    {
        public BookValidator() 
        {
            RuleFor(w => w.Id)
                .NotEmpty();

            RuleFor(w => w.Title)
                .NotEmpty()
                .MaximumLength(255); //In a real scenario you would probably want some limitation on how long a title should be, putting 255 here as a placeholder

            RuleFor(w => w.Author)
                .NotEmpty()
                .MaximumLength(255); //In a real scenario you would probably want some limitation on how long an author's name should be, putting 255 here as a placeholder

            RuleFor(w => w.ISBN)
                .NotEmpty()
                .Length(13); //From what I saw online ISBNs are meant to be exactly 13 digits
        }  
    }
}

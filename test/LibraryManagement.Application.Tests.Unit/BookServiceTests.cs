using Bogus;
using FluentAssertions;
using FluentValidation;
using LibraryManagement.Application.Models;
using LibraryManagement.Application.Repositories;
using LibraryManagement.Application.Repositories.Interfaces;
using LibraryManagement.Application.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Application.Tests.Unit
{
    public class BookServiceTests
    {
        private readonly BookService _sut;
        private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();
        private readonly IValidator<Book> _bookValidator = Substitute.For<IValidator<Book>>();

        private readonly Faker<Book> _bookGenerator = new Faker<Book>()
            .RuleFor(b => b.Id, faker => faker.Random.Guid())
            .RuleFor(b => b.Title, faker => faker.Random.Word())
            .RuleFor(b => b.Author, faker => faker.Random.Words(2))
            .RuleFor(b => b.ISBN, faker => faker.Random.ReplaceNumbers("#############"))
            .RuleFor(b => b.IsAvailable, true);

        public BookServiceTests()
        {
            _sut = new BookService(_bookRepository, _bookValidator);
        }

        [Fact(Timeout = 2000)]
        public async Task CreateAsync_ShouldCreateBook_WhenBookIsValid()
        {
            //Arrange
            var book = _bookGenerator.Generate();

            _bookRepository.CreateAsync(book).Returns(true);

            //Act
            var result = await _sut.CreateAsync(book);

            //Assert
            result.Should().Be(true);
        }

        [Fact(Timeout = 2000)]
        public async Task GetAsync_ShouldReturnBook_WhenBookExists()
        {
            //Arrange
            var book = _bookGenerator.Generate();

            _bookRepository.GetAsync(book.Id).Returns(book);

            //Act
            var result = await _sut.GetAsync(book.Id);

            //Assert
            result.Should().BeEquivalentTo(book);
        }

        [Fact(Timeout = 2000)]
        public async Task GetAsync_ShouldNotReturnUser_WhenUserDoesNotExist()
        {
            //Arrange
            var bookId = Guid.NewGuid();

            _bookRepository.GetAsync(bookId).Returns(null as Book);

            //Act
            var result = await _sut.GetAsync(bookId);

            //Assert
            result.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task GetAllAsync_ShouldReturnListOfBooks_WhenBooksExist()
        {
            var books = _bookGenerator.Generate(10);

            _bookRepository.GetAllAsync().Returns(books);

            var result = await _sut.GetAllAsync();

            result.Should().BeEquivalentTo(books);
        }

        [Fact(Timeout = 2000)]
        public async Task GetAllAsync_ShouldReturnEmptyList_WhenBooksDoNotExist()
        {
            List<Book> books = [];

            _bookRepository.GetAllAsync().Returns(books);

            var result = await _sut.GetAllAsync();

            result.Should().BeEmpty();
            result.Should().BeEquivalentTo(books);
        }

    }
}

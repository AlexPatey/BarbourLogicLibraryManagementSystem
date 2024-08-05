using Bogus;
using FluentAssertions;
using FluentValidation;
using LibraryManagement.Application.Models;
using LibraryManagement.Application.Repositories.Interfaces;
using LibraryManagement.Application.Services;
using NSubstitute;

namespace LibraryManagement.Application.Tests.Unit
{
    public class UserServiceTests
    {
        private readonly UserService _sut;
        private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
        private readonly IBookRepository _bookRepository = Substitute.For<IBookRepository>();
        private readonly IValidator<User> _userValidator = Substitute.For<IValidator<User>>();

        private readonly Faker<User> _userGenerator = new Faker<User>()
            .RuleFor(u => u.Id, faker => faker.Random.Guid())
            .RuleFor(u => u.Name, faker => faker.Random.Words(2))
            .RuleFor(u => u.Books, []);

        public UserServiceTests()
        {
            _sut = new UserService(_userRepository, _bookRepository, _userValidator);
        }

        [Fact(Timeout = 2000)]
        public async Task CreateAsync_ShouldCreateUser_WhenUserIsValid()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.CreateAsync(user).Returns(true);

            //Act
            var result = await _sut.CreateAsync(user);

            //Assert
            result.Should().Be(true);
        }

        [Fact(Timeout = 2000)]
        public async Task GetAsync_ShouldReturnUser_WhenUserExists()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            //Act
            var result = await _sut.GetAsync(user.Id);

            //Assert
            result.Should().BeEquivalentTo(user);
        }

        [Fact(Timeout = 2000)]
        public async Task GetAsync_ShouldNotReturnUser_WhenUserDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid();

            _userRepository.GetAsync(userId).Returns(null as User);

            //Act
            var result = await _sut.GetAsync(userId);

            //Assert
            result.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task DeleteByIdAsync_ShouldDeleteUser_WhenUserExists()
        {
            //Arrange
            var userId = Guid.NewGuid();

            _userRepository.GetAsync(userId).Returns(new User
            {
                Id = userId,
                Name = "Name",
                Books = []
            });

            _userRepository.DeleteByIdAsync(userId).Returns(true);

            //Act
            var result = await _sut.DeleteByIdAsync(userId);

            //Assert
            result.Should().Be(true);
        }

        [Fact(Timeout = 2000)]
        public async Task DeleteByIdAsync_ShouldReturnFalse_WhenUserDoesNotExist()
        {
            //Arrange
            var userId = Guid.NewGuid();

            _userRepository.GetAsync(userId).Returns(null as User);

            //Act
            var result = await _sut.DeleteByIdAsync(userId);

            //Assert
            result.Should().Be(false);
        }

        [Fact(Timeout = 2000)]
        public async Task LendBookAsync_ShouldReturnSuccessfulUserResult_WhenUserLendsBook()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = true
            };

            _bookRepository.GetAsync(book.Id).Returns(book);

            _userRepository.UpdateUsersBooksAsync(user).Returns(true);

            _bookRepository.UpdateAsync(book).Returns(true);

            //Act
            var result = await _sut.LendBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Error.Should().BeEmpty();
            result.Model.Should().NotBeNull();
            result.Model!.Books.Should().HaveCount(1);
        }

        [Fact(Timeout = 2000)]
        public async Task LendBookAsync_ShouldReturnUnsuccessfulUserResult_WhenUserCannotBeFound()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            _userRepository.GetAsync(userId).Returns(null as User);

            //Act
            var result = await _sut.LendBookAsync(userId, bookId);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"User with Id {userId} could not be found.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task LendBookAsync_ShouldReturnUnsuccessfulUserResult_WhenBookCannotBeFound()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var bookId = Guid.NewGuid();

            _bookRepository.GetAsync(bookId).Returns(null as Book);

            //Act
            var result = await _sut.LendBookAsync(user.Id, bookId);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"Book with Id {bookId} could not be found.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task LendBookAsync_ShouldReturnUnsuccessfulUserResult_WhenBookIsUnavailableToLend()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = false
            };

            _bookRepository.GetAsync(book.Id).Returns(book);

            //Act
            var result = await _sut.LendBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"Book with Id {book.Id} is not available to lend.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task LendBookAsync_ShouldReturnUnsuccessfulUserResult_WhenUserFailsToUpdate()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = true
            };

            _bookRepository.GetAsync(book.Id).Returns(book);

            _userRepository.UpdateUsersBooksAsync(user).Returns(false);

            //Act
            var result = await _sut.LendBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"Could not update user with Id {user.Id}.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task LendBookAsync_ShouldReturnUnsuccessfulUserResult_WhenBookFailsToUpdate()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = true
            };

            _bookRepository.GetAsync(book.Id).Returns(book);

            _userRepository.UpdateUsersBooksAsync(user).Returns(true);
            _bookRepository.UpdateAsync(book).Returns(false);

            //Act
            var result = await _sut.LendBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"Could not update book with Id {book.Id}.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task ReturnBookAsync_ShouldReturnSuccessfulUserResult_WhenUserLendsBook()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = true
            };

            user.Books.Add(book);

            _bookRepository.GetAsync(book.Id).Returns(book);

            _userRepository.UpdateUsersBooksAsync(user).Returns(true);

            _bookRepository.UpdateAsync(book).Returns(true);

            //Act
            var result = await _sut.ReturnBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Error.Should().BeEmpty();
            result.Model.Should().NotBeNull();
            result.Model!.Books.Should().HaveCount(0);
        }

        [Fact(Timeout = 2000)]
        public async Task ReturnBookAsync_ShouldReturnUnsuccessfulUserResult_WhenUserCannotBeFound()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var bookId = Guid.NewGuid();

            _userRepository.GetAsync(userId).Returns(null as User);

            //Act
            var result = await _sut.ReturnBookAsync(userId, bookId);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"User with Id {userId} could not be found.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task ReturnBookAsync_ShouldReturnUnsuccessfulUserResult_WhenBookCannotBeFound()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = true
            };

            _bookRepository.GetAsync(book.Id).Returns(null as Book);

            user.Books.Add(book);

            //Act
            var result = await _sut.ReturnBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"Book with Id {book.Id} could not be found.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task ReturnBookAsync_ShouldReturnUnsuccessfulUserResult_WhenUserDoesNotHaveBook()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = false
            };

            _bookRepository.GetAsync(book.Id).Returns(book);

            //Act
            var result = await _sut.ReturnBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"User with Id {user.Id} does not have book with Id {book.Id}.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task ReturnBookAsync_ShouldReturnUnsuccessfulUserResult_WhenUserFailsToUpdate()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = true
            };

            _bookRepository.GetAsync(book.Id).Returns(book);

            user.Books.Add(book);

            _userRepository.UpdateUsersBooksAsync(user).Returns(false);

            //Act
            var result = await _sut.ReturnBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"Could not update user with Id {user.Id}.");
            result.Model.Should().BeNull();
        }

        [Fact(Timeout = 2000)]
        public async Task ReturnBookAsync_ShouldReturnUnsuccessfulUserResult_WhenBookFailsToUpdate()
        {
            //Arrange
            var user = _userGenerator.Generate();

            _userRepository.GetAsync(user.Id).Returns(user);

            var book = new Book
            {
                Id = Guid.NewGuid(),
                Title = "Title",
                Author = "Author",
                ISBN = "1234567890123",
                IsAvailable = true
            };

            _bookRepository.GetAsync(book.Id).Returns(book);

            user.Books.Add(book);

            _userRepository.UpdateUsersBooksAsync(user).Returns(true);
            _bookRepository.UpdateAsync(book).Returns(false);

            //Act
            var result = await _sut.ReturnBookAsync(user.Id, book.Id);

            //Assert
            result.Should().NotBeNull();
            result.Success.Should().BeFalse();
            result.Error.Should().Be($"Could not update book with Id {book.Id}.");
            result.Model.Should().BeNull();
        }
    }
}

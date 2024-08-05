using FluentValidation;
using LibraryManagement.Application.Repositories;
using LibraryManagement.Application.Repositories.Interfaces;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryManagement.Application.Extensions
{
    public static class ApplicationServiceCollectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<IUserService, UserService>();
            services.AddSingleton<IBookRepository, BookRepository>(); /*The repository dependencies have to be singleton as otherwise the book and user lists used to represent Db tables will not be
                                                                      persisted across API calls, in a real scenario I would set these dependencies to the scoped lifetime, the singleton lifetime
                                                                      could cause concurrency problems if we were using a real Db*/
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Scoped);
            return services;
        }
    }
}

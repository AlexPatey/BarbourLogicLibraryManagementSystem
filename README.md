# Barbour Logic - Library Management System Api (Solution to "Development Task 1")

I have provided an MVC-style REST API using .NET Core 8.

In order to run the API, please pull the API onto your local development machine, and run it in Visual Studio 2022 (or another IDE of your choice).

I have configured Swagger UI so that you can query the various API endpoints from /swagger/index.html.

In order to run the unit tests, you can use the dotnet test command from the Developer Powershell window in VS 2022, or right-click on the LibraryManagement.Application.Tests.Unit project (in the "test" folder) and select "Run Tests."

The solution I have provided covers all of the main requirements provided.

I have provided comments across the code to explain various decisions I have made, etc.

I have used the following third-party libraries:
- XUnit for my testing framework.
- FluentValidation for my model validation.
- FluentAssertions to assert against test results.
- Bogus to generate test data.
- NSubstitute to mock up dependencies in my unit tests.

Please note I also have a Writings API project on my GitHub if you wish to see an example of a more fleshed-out API including EF Core, Serilog for logging, etc.

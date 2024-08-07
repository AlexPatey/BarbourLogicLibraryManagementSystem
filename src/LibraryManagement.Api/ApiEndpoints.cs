﻿namespace LibraryManagement.Api
{
    public class ApiEndpoints
    {
        private const string ApiBase = "api";

        public static class Books
        {
            private const string Base = $"{ApiBase}/books";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string GetAll = Base;
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
        }

        public static class Users
        {
            private const string Base = $"{ApiBase}/users";

            public const string Create = Base;
            public const string Get = $"{Base}/{{id:guid}}";
            public const string Update = $"{Base}/{{id:guid}}";
            public const string Delete = $"{Base}/{{id:guid}}";
            public const string LendBook = $"{Base}/{{userId:guid}}/lend/{{bookId:guid}}";
            public const string ReturnBook = $"{Base}/{{userId:guid}}/return/{{bookId:guid}}";
        }
    }
}

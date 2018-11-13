using Aproplan.Api.Model.Actors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aproplan.Api.Tests.Utilities
{
    public static class UserUtility
    {
        public static User CreateUser(string alias, string displayName, string languageCode = "en")
        {
            return new User
            {
                DisplayName = displayName,
                Alias = alias,
                DefaultEmail = alias,
                Id = Guid.NewGuid(),
                LanguageCode = languageCode,
                Person = new Person
                {
                    Name = displayName,
                    LanguageCode = languageCode,
                    Id = Guid.NewGuid(),
                    Code = displayName
                },
            };
        }
    }
}

using BookStoreEcommerce.Models.Entities;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BookStoreEcommerce.Utilities
{
    public static class SessionHelper
    {
        private const string UserSessionKey = "CurrentUser";

        public static void SetUser(ISession session, User user)
        {
            var opts = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles,
                // optional: PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                // optional: MaxDepth = 32
            };

            session.SetString(UserSessionKey, JsonSerializer.Serialize(user, opts));
        }

        public static User? GetCurrentUser(ISession session)
        {
            var userJson = session.GetString(UserSessionKey);
            if (string.IsNullOrEmpty(userJson))
                return null;

            var opts = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.IgnoreCycles
            };
            return JsonSerializer.Deserialize<User>(userJson, opts);
        }

        public static bool IsLoggedIn(ISession session)
        {
            return GetCurrentUser(session) != null;
        }

        public static bool IsAdmin(ISession session)
        {
            var user = GetCurrentUser(session);
            return user?.UserType == UserType.Admin;
        }

        public static bool IsCustomer(ISession session)
        {
            var user = GetCurrentUser(session);
            return user?.UserType == UserType.Customer;
        }

        public static void ClearSession(ISession session)
        {
            session.Clear();
        }
    }
}
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public interface IUserRepository
    {
        int UserCount { get; }

        bool IsExistingUser(string id);

        bool TryGetUser(string id, out User user);

        void AddUser(User user);
    }
}
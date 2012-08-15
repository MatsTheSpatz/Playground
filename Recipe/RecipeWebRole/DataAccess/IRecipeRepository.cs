using System.Collections.Generic;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public interface IRecipeRepository
    {
            //void AddRecipe(int id, string text);

            //IEnumerable<int> GetRecipeIds();

        void SetRecipe(Recipe recipe);

        Recipe GetRecipe(int id);

        IList<int> GetRecipeIds();
    }
}
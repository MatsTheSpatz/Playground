using System.Collections.Generic;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public interface IRecipeRepository
    {
        void SetRecipe(Recipe recipe);

        Recipe GetRecipe(int id);

        IList<int> GetRecipeIds();
    }
}
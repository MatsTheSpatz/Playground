using System;
using System.Collections.Generic;
using System.Linq;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public class InMemoryRecipeRepository : IRecipeRepository
    {
        private static readonly List<Recipe> _recipes;

        static InMemoryRecipeRepository()
        {
            _recipes = new List<Recipe>();

            _recipes.Add(new TextRecipe() { Id = 3412, Name = "Bunte Smarties" });
            _recipes.Add(new TextRecipe() { Id = 1399, Name = "BigMac mit fetten Pommes und Cola" });
            _recipes.Add(new TextRecipe() { Id = 7100, Name = "Truffes du jour an Bärlauchsauce" });
        }

        public IList<int> GetRecipeIds()
        {
            return _recipes.Select(r => r.Id).ToList();
        }

        public Recipe GetRecipe(int id)
        {
            return _recipes.FirstOrDefault(r => r.Id == id);
        }

        public void SetRecipe(Recipe recipe)
        {
            if (recipe.Id <= 0)
            {
                recipe.Id = CreateUniqueRecipeId();
                _recipes.Add(recipe);
            }
            else
            {
                Recipe oldRecipe = GetRecipe(recipe.Id);
                if (oldRecipe != null)
                {
                    _recipes.Remove(oldRecipe);
                }
                _recipes.Add(recipe);
            }
        }

        private int CreateUniqueRecipeId()
        {
            var random = new Random();
            while (true)
            {
                int testId = random.Next(0, 100000);
                if (_recipes.Find(r => r.Id == testId) == null)
                {
                    return testId;
                }               
            }
        }
    
    }
}
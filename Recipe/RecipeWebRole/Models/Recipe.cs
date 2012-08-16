using System;
using System.Xml.Serialization;

namespace RecipeWebRole.Models
{
    [Serializable]
    public abstract class Recipe
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
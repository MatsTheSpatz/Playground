using System;
using System.Xml.Serialization;

namespace RecipeWebRole.Models
{
    [Serializable]
    public abstract class Recipe
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Author { get; set; }

        public string AuthorId { get; set; }

        public DateTime CreationDate { get; set; }

        public DateTime? ModificationDate { get; set; }

        public string Description { get; set; }

        public string Quantity { get; set; }

        public string PreparationTime { get; set; }

        public string CookingTime { get; set; }

        public bool IsVegetarian { get; set; }

        public bool IsSuitedAsPresent { get; set; }

        public DishCategory[] DishCategories { get; set; }

        public Season[] Seasons { get; set; }

        public SkillLevel SkillLevel { get; set; }
    }

    public enum Season
    {
        Spring,
        Summer,
        Fall,
        Winter
    }

    public enum DishCategory
    {
        Aperitif,
        Soup,
        Starter,
        Maincourse,
        Dessert,
        Cookies,
        Other
    }

    public enum SkillLevel
    {
        Novice = 1,
        Beginner = 2,
        Average = 3,
        Advanced = 4,
        Expert = 5
    }

}
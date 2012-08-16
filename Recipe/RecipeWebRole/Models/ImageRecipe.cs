using System;
using System.Xml.Serialization;

namespace RecipeWebRole.Models
{
    [Serializable]
    [XmlRoot("ImageRecipe", Namespace = "http://www.matsbader.com", IsNullable = false)]
    public class ImageRecipe : Recipe
    {
        public int ImageId { get; set; }

        public string ScannedText { get; set; }
    }
}
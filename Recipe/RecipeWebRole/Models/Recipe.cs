using System;
using System.Xml.Serialization;

namespace RecipeWebRole.Models
{
    [Serializable]
    [XmlRoot("Recipe", Namespace = "http://www.matsbader.com", IsNullable = false)]
    public class Recipe
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [XmlArray("IngredientSections")]
        public IngredientSection[] Ingredients { get; set; }

        [XmlArray("Instructions")]
        public Instruction[] Instructions { get; set; }
    }

    [Serializable]
    public class IngredientSection
    {
        public string SectionHeader { get; set; }

//        [XmlArrayAttribute("Ingredients")]
        public string[] Items { get; set; }
    }

    [Serializable]
    public class Instruction
    {
        [XmlElement(IsNullable = false)]
        public string Text { get; set; }

        [XmlAttribute("IndentationLevel")]
        public int Level { get; set; }
    }
}
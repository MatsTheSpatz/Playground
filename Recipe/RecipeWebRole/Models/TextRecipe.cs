using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace RecipeWebRole.Models
{
    [Serializable]
    [XmlRoot("TextRecipe", Namespace = "http://www.matsbader.com", IsNullable = false)]
    public class TextRecipe : Recipe
    {
        public TextRecipe()
        {
            SkillLevel = SkillLevel.Average;
        }

        [XmlArray("IngredientSections")]
        public IngredientSection[] IngredientSections { get; set; }

        [XmlArray("Instructions")]
        public Instruction[] Instructions { get; set; }
    }


    [Serializable]
    public class IngredientSection
    {
        public string SectionHeader { get; set; }

        [XmlArrayAttribute("IngredientItems")]
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

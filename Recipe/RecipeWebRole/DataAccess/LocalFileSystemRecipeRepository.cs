using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public class LocalFileSystemRecipeRepository : IRecipeRepository
    {
        private static readonly LocalFileSystemRecipeRepository _instance = new LocalFileSystemRecipeRepository();

        private const string FolderName = @"c:/temp/";
        private const string RecipeMarker = "recipe_";

        public static LocalFileSystemRecipeRepository Instance
        {
            get { return _instance; }
        }

        private LocalFileSystemRecipeRepository()
        {
            // singleton > keep private
        }

        public void SetRecipe(RecipeWebRole.Models.Recipe recipe)
        {
            if (recipe.Id <= 0)
            {
                recipe.Id = CreateRecipeId();
            }

            SerializeToXml(recipe, GetFilePath(recipe.Id));
        }

        public RecipeWebRole.Models.Recipe GetRecipe(int id)
        {
            return DeserializeFromXml(GetFilePath(id));
        }

        public IList<int> GetRecipeIds()
        {
            var recipeIds = new List<int>();
            string[] filePaths = Directory.GetFiles(FolderName, "*.xml", SearchOption.TopDirectoryOnly);
            foreach (string filePath in filePaths)
            {
                string fileName = Path.GetFileName(filePath);
                if (fileName.StartsWith(RecipeMarker, StringComparison.InvariantCultureIgnoreCase))
                {
                    int startIdx = RecipeMarker.Length;
                    int endIdx = fileName.Length - ".xml".Length;
                    string recipeId = fileName.Substring(startIdx, endIdx - startIdx);
                    recipeIds.Add(Int32.Parse(recipeId));
                }
            }
            return recipeIds;
        }

        private int CreateRecipeId()
        {
            var random = new Random();
            IList<int> usedIds = GetRecipeIds();
            while (true)
            {
                int id = random.Next(999) + 1000;
                if (usedIds.Contains(id))
                {
                    return id;
                }
            }
        }

        private string GetFilePath(int id)
        {
            return (FolderName + RecipeMarker + id + ".xml");
        }

        private static void SerializeToXml(Recipe recipe, string fileName)
        {
            var serializer = new XmlSerializer(typeof(Recipe));
            using (TextWriter writer = new StreamWriter(fileName))
            {
                serializer.Serialize(writer, recipe);
            }
        }

        private static Recipe DeserializeFromXml(string fileName)
        {
            var serializer = new XmlSerializer(typeof(RecipeWebRole.Models.Recipe));
            using (var fs = new FileStream(fileName, FileMode.Open))
            {
                var recipe = (Recipe) serializer.Deserialize(fs);
                return recipe;
            }
        }
    }
}
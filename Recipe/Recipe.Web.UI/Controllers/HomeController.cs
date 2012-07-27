using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using System.Threading;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace Recipe.Web.UI.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Welcome to ASP.NET MVC!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }

        [HttpPost]
        public JsonResult SetRecipe(Recipe recipe)
        {
            string folderName = @"c:/temp/";

            SerializeToXml(recipe, folderName + "recipe1.xml");
            //dynamic obj = new ExpandoObject();
            //obj.Value = 10;

            //string a =
            //    "{\"ingredients\":[{\"sectionHeader\":\"\",\"items\":[]}],\"instructions\":[{\"text\":\"Hier kommt dann mal der Text\",\"level\":0},{\"text\":\"Hier kommt Roger Federer\",\"level\":0}]}";

            //var jss = new JavaScriptSerializer();
            //object o = jss.Deserialize(a, typeof(Recipe));


            //NameValueCollection z = Request.Form;
            //foreach (string key in z.AllKeys)
            //{
            //    string jsonText = z[key];
            //    var jss = new JavaScriptSerializer();
            //    object o = jss.Deserialize(jsonText, typeof(Recipe));
            //    System.Diagnostics.Debug.WriteLine(o);
            //}
            Thread.Sleep(1000);





          //  throw new ArgumentException("Bad things happended");
            return Json(recipe);
        }


        private static void SerializeToXml(Recipe recipe, string fileName)
        {
            var serializer = new XmlSerializer(typeof(Recipe));
            TextWriter writer = new StreamWriter(fileName);
            serializer.Serialize(writer, recipe);
            writer.Close();
        }

        private static Recipe DeserializeFromXml(string fileName)
        {
            var serializer = new XmlSerializer(typeof(Recipe));
            var fs = new FileStream(fileName, FileMode.Open);
            var recipe = (Recipe) serializer.Deserialize(fs);
            return recipe;
        }
    }



    [Serializable]
    [XmlRoot("Recipe", Namespace = "http://www.matsbader.com", IsNullable = false)]
    public class Recipe
    {
        [XmlArrayAttribute("IngredientSections")]
        public IngredientSection[] Ingredients { get; set; }

        [XmlArrayAttribute("Instructions")]
        public Instruction[] Instructions { get; set; }
    }

    [Serializable]
    public class IngredientSection
    {
        public string SectionHeader { get; set; }

        [XmlArrayAttribute("Ingredients")]
        public string[] Items;
    }

    [Serializable]
    public class Instruction
    {
        [XmlElementAttribute(IsNullable = false)]
        public string Text { get; set; }

        [XmlAttribute("IndentationLevel")]
        public int Level { get; set; }
    }

}

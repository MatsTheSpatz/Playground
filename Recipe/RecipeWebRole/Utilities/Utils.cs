using System.Web.Script.Serialization;

namespace RecipeWebRole.Utilities
{
    public static class Utils
    {
        /// <summary>
        /// Converts the object to json.
        /// </summary>
        /// <remarks>
        /// You might not need this method. You can use Json.Encode(obj) in a Razor-view, e.g.
        /// @Html.Raw(Json.Encode(recipe));
        /// </remarks>
        public static string ConvertObjectToJson<T>(T obj)
        {
            var jss = new JavaScriptSerializer();
            string jsonText = jss.Serialize(obj);

            // if you want to insert the text into a razor view, use:
            // @Html.Raw(jsonText); 

            return jsonText;
        }

        /// <summary>
        /// Converts the json to object of type T.
        /// </summary>
        /// <remarks>
        /// You might not need this method. A controller does the same for you. E.g. use signature
        ///    [HttpPost]
        ///    public JsonResult SetRecipe(Recipe recipe)
        ///    {  ... }
        /// This will only work if the Http-message define
        ///   dataType: 'json'
        ///   contentType: 'application/json; charset=utf-8'
        /// </remarks>
        public static T ConvertJsonToObject<T>(string jsonText)
        {
            var jss = new JavaScriptSerializer();
            object obj = jss.Deserialize(jsonText, typeof (T));
            return (T) obj;
        }
    }
}
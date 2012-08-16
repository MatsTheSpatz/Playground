using System;
using System.Text;

namespace RecipeOcr
{
    [Flags]
    public enum TextType
    {
        Normal = 0x01,
        Handprinted = 0x02, // Works only for field-level recognition
        Gothic = 0x04
    }

    public static class TextTypesExtensions
    {
        public static string AsUrlParams(this TextType textTypes)
        {
            var result = new StringBuilder();

            if (textTypes.HasFlag(TextType.Normal))
            {
                appendToResult(result, "normal");
            }

            if (textTypes.HasFlag(TextType.Handprinted))
            {
                appendToResult(result, "handprinted");
            }

            if (textTypes.HasFlag(TextType.Gothic))
            {
                appendToResult(result, "gothic");
            }

            return result.ToString();
        }

        private static void appendToResult(StringBuilder result, string value)
        {
            if (result.Length > 0)
            {
                result.Append(",");                
            }
            result.Append(value);
        }
    }
}

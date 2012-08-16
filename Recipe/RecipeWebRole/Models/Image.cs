using System.Drawing.Imaging;

namespace RecipeWebRole.Models
{
    public class Image
    {
        public int Id { get; set; }

        public byte[] Data { get; set; }

//        public ImageFormat Format { get; set; }
    }
}
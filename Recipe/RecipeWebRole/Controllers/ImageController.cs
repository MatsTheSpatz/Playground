using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;

namespace RecipeWebRole.Controllers
{
    public class ImageController : Controller
    {
        private readonly IImageRepository _imageRepo;

        public ImageController(IImageRepository imageRepo)
        {
            _imageRepo = imageRepo;
        }


        //
        // GET: /Image/

        public ActionResult Get(int imageId)
        {
            Image image = _imageRepo.GetImage(imageId);
            return new ImageResult(image.Data, "image/jpeg");
        }
    }

    public class ImageResult : ActionResult
    {
        public ImageResult(byte[] sourceStream, String contentType)
        {
            ImageBytes = sourceStream;
            ContentType = contentType;
        }

        public String ContentType
        {
            get; set;
        }

        public byte[] ImageBytes
        {
            get; set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var response = context.HttpContext.Response;
            response.Clear();
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = ContentType;

            using (var memoryStream = new MemoryStream(ImageBytes))
            {
                memoryStream.WriteTo(response.OutputStream);
            }
        }
    }
}

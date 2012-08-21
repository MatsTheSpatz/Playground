using System;
using System.Dynamic;
using System.IO;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using RecipeWebRole.DataAccess;
using RecipeWebRole.Models;

namespace RecipeWebRole.Controllers
{
    public class ScannedRecipeEditorController : Controller
    {
        private readonly IRecipeRepository _recipeRepo;
        private readonly IImageRepository _imageRepo;
        private readonly IOcrService _ocrService;

        public ScannedRecipeEditorController(
            IRecipeRepository recipeRepo, 
            IImageRepository imageRepo,
            IOcrService ocrService)
        {
            _recipeRepo = recipeRepo;
            _imageRepo = imageRepo;
            _ocrService = ocrService;
        }


        //
        // GET: /ScannedRecipeEditor/OpenNew

        [HttpGet]
        public ActionResult OpenNew(int imageId)
        {
            string text = _ocrService.GetOcrResult(imageId);
            var recipe = new ImageRecipe {ImageId = imageId, ScannedText = text };


            return View(recipe);
        }


        //
        // GET: /ScannedRecipeEditor/OpenExisting

        [HttpGet]
        public ActionResult OpenExisting(int recipeId)
        {
            var recipe = (ImageRecipe) _recipeRepo.GetRecipe(recipeId);
            if (recipe == null)
            {
                throw new ArgumentException("Invalid recipe Id.");
            }

            return View(recipe);
        }


        //
        // GET: /ScannedRecipeEditor/UploadImage

        [HttpGet]
        public ActionResult UploadImage()
        {
            return View();
        }


        //
        // POST: /ScannedRecipeEditor/UploadImage

        [HttpPost]
        public ActionResult UploadImage(HttpPostedFileBase file)
        {
            // Create image from uploaded file.
            Image image;
            using (var memoryStream = new MemoryStream())
            {
                file.InputStream.CopyTo(memoryStream);
                image = new Image {Data = memoryStream.ToArray()};
            }

            // Save image
            _imageRepo.SetImage(image);

            return RedirectToAction("ProcessImage", new {imageId = image.Id});
        }


        //
        // GET: /ScannedRecipeEditor/ProcessImage

        [HttpGet]
        public ActionResult ProcessImage(int imageId)
        {
            // Call OCR service on background thread.
            // The call is blocking until OCR finished.
            Image image = _imageRepo.GetImage(imageId);
            ThreadPool.QueueUserWorkItem(obj => _ocrService.ProcessImage(obj as Image), image);

            ViewBag.ImageId = imageId;
            return View();
        }


        //
        // GET: /ScannedRecipeEditor/IsOcrComplete

        [HttpGet]
        public JsonResult IsOcrComplete(int imageId)
        {
            dynamic obj = new ExpandoObject();
            obj.isFinished = _ocrService.IsOcrFinished(imageId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }
    }
}

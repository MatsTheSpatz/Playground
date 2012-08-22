using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using RecipeWebRole.DataAccess;

namespace RecipeWebRole.Controllers
{
    public class CustomControllerFactory : DefaultControllerFactory
    {
         //       private static readonly IRecipeRepository _repo = new InMemoryRecipeRepository();//new FakeInMemoryRecipeRepository();
        //private static readonly LocalFileSystemRecipeRepository _repo = LocalFileSystemRecipeRepository.Instance;

        private static readonly IRecipeRepository _recipeRepo = new BlobStorageRecipeRepository();
        private static readonly IImageRepository _imageRepo = new BlobStorageImageRepository();
        private static readonly IUserRepository _userRepo = UserRepository.Instance;
        private static readonly IOcrService _ocrService = new OcrService();

        public override IController CreateController(RequestContext requestContext, string controllerName)
        {
            switch (controllerName)
            {
                case "Account":
                    return new AccountController(_userRepo);

                case "Home": 
                    return new HomeController(_recipeRepo, _userRepo);
                
                case "ScannedRecipeEditor":
                    return new ScannedRecipeEditorController(_recipeRepo, _imageRepo, _ocrService);
               
                case "BlankRecipeEditor": 
                    return new BlankRecipeEditorController(_recipeRepo);

                case "Image":
                    return new ImageController(_imageRepo);

                default:
                    return base.CreateController(requestContext, controllerName);
            }                
        }
    }
}
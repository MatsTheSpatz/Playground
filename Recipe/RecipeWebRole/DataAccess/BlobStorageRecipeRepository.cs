using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public class BlobStorageRecipeRepository : IRecipeRepository
    {
        private const string BlobContainer = "recipecontainer";  // must be lowercase!!!

        public IList<int> GetRecipeIds()
        {
            CloudBlobContainer container = GetBlobContainer();

            // Loop over blobs within the container and output the URI to each of them
            var ids = new List<string>();
            foreach (IListBlobItem blobItem in container.ListBlobs())
            {
                ids.Add(blobItem.Uri.Segments.Last());
            }

            return ids.Select(Int32.Parse).ToList();
        }

        public Recipe GetRecipe(int id)
        {
            // Retrieve reference to blob.
            CloudBlobContainer container = GetBlobContainer();
            string blobId = id.ToString(CultureInfo.InvariantCulture);
            CloudBlob blob = container.GetBlobReference(blobId);

            // get data (if it exists)
            try
            {
                byte[] recipeData = blob.DownloadByteArray();

                string rType = blob.Metadata.Get("RType");
                Type recipeType = (rType == "Image") ? typeof(ImageRecipe) : typeof(TextRecipe);
                
                Recipe recipe = DeserializeFromXml(recipeData, recipeType);
                return recipe;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetRecipe(Recipe recipe)
        {
            if (recipe.Id <= 0)
            {
                recipe.Id = CreateUniqueRecipeId();
            }

            bool isImageRecipe = recipe is ImageRecipe;
            if (isImageRecipe && (recipe as ImageRecipe).ImageId <= 0)
            {
                throw new ArgumentException("Image Id is mandatory for an ImageRecipe.");
            }

            // Retrieve reference to blob.
            CloudBlobContainer container = GetBlobContainer();
            string blobId = recipe.Id.ToString(CultureInfo.InvariantCulture);
            CloudBlob blob = container.GetBlobReference(blobId);
           
            // Uplaod recipe
            byte[] recipeData = SerializeToXml(recipe, isImageRecipe ? typeof(ImageRecipe) : typeof(TextRecipe));
            blob.UploadByteArray(recipeData);
            blob.Metadata.Add("RType", (recipe is ImageRecipe) ? "Image" : "Text");
            blob.SetMetadata();
        }

        private int CreateUniqueRecipeId()
        {
            // TODO: do this differently 
            // (see for example http://blog.tatham.oddie.com.au/2011/07/14/released-snowmaker-a-unique-id-generator-for-azure-or-any-other-cloud-hosting-environment/
            
            var random = new Random();
            IList<int> recipeIds = GetRecipeIds();

            int maxNumberOfTrials = 100;
            while (maxNumberOfTrials-- > 0)
            {
                int testId = 1000 + random.Next(0, 8999);
                if (!recipeIds.Contains(testId))
                {
                    return testId;
                }               
            }

            throw new InvalidOperationException("Unable to create unique ID.");
        }
        private CloudBlobContainer GetBlobContainer()
        {
            CloudBlobClient blobClient = GetCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(BlobContainer);

            container.CreateIfNotExist();
            container.SetPermissions(new BlobContainerPermissions {PublicAccess = BlobContainerPublicAccessType.Blob});
            return container;
        }

        private CloudBlobClient GetCloudBlobClient()
        {
            // Retrieve storage account from connection-string
            string setting = CloudConfigurationManager.GetSetting("StorageConnectionString");
            System.Diagnostics.Debug.WriteLine(setting);
            //CloudStorageAccount storageAccount = CloudStorageAccount.DevelopmentStorageAccount;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            return blobClient;
        }

        private static byte[] SerializeToXml(Recipe recipe, Type recipeType)
        {
            var serializer = new XmlSerializer(recipeType);
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, recipe);
                return memoryStream.GetBuffer();
            }
        }

        private static Recipe DeserializeFromXml(byte[] buffer, Type recipeType)
        {
            var serializer = new XmlSerializer(recipeType);
            using (var memoryStream = new MemoryStream(buffer))
            {
                var recipe = (Recipe)serializer.Deserialize(memoryStream);
                return recipe;
            }
        }
    }
}
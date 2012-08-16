using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    public class BlobStorageImageRepository : IImageRepository
    {
        private const string BlobContainer = "imagecontainer";  // must be lowercase!!!

        public IList<int> GetImageIds()
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

        public Image GetImage(int id)
        {
            // Retrieve reference to blob.
            CloudBlobContainer container = GetBlobContainer();
            string blobId = id.ToString(CultureInfo.InvariantCulture);
            CloudBlob blob = container.GetBlobReference(blobId);

            // get data (if it exists)
            try
            {
                byte[] data = blob.DownloadByteArray();
                return new Image() { Id = id, Data = data};
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void SetImage(Image image)
        {
            if (image.Id <= 0)
            {
                image.Id = CreateUniqueId();
            }

            // Retrieve reference to blob.
            CloudBlobContainer container = GetBlobContainer();
            string blobId = image.Id.ToString(CultureInfo.InvariantCulture);
            CloudBlob blob = container.GetBlobReference(blobId);
           
            // Uplaod recipe
            blob.UploadByteArray(image.Data);
        }

        private int CreateUniqueId()
        {
            // TODO: do this differently 
            // (see for example http://blog.tatham.oddie.com.au/2011/07/14/released-snowmaker-a-unique-id-generator-for-azure-or-any-other-cloud-hosting-environment/
            
            var random = new Random();
            IList<int> recipeIds = GetImageIds();

            int maxNumberOfTrials = 100;
            while (maxNumberOfTrials-- > 0)
            {
                int testId = random.Next(0, 100000);
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

    }
}
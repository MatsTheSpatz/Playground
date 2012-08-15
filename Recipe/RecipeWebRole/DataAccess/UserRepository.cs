using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using RecipeWebRole.Models;

namespace RecipeWebRole.DataAccess
{
    [Serializable]
    public class UserData
    {
        public User[] Users { get; set; }
    }

    public class UserRepository : IUserRepository
    {
        private const string ContainerName = "usercontainer";  // must be lowercase!!!
        private const string BlobName = "users";

        private static readonly UserRepository _instance;
        private List<User> _users;

        static UserRepository()
        {
            _instance = new UserRepository();
        }

        private UserRepository()
        {
        }

        public static UserRepository Instance
        {
            get { return _instance; }
        }

        public int UserCount
        {
            get { return Users.Count; }
        }

        public bool IsExistingUser(string id)
        {
            User user;
            return TryGetUser(id, out user);
        }

        public bool TryGetUser(string id, out User user)
        {
            user = Users.FirstOrDefault(u => u.Id == id);
            return (user != null);
        }

        public void AddUser(User user)
        {
            User existingUser;
            if (TryGetUser(user.Id, out existingUser))
            {
                // change name
                existingUser.Name = user.Name;
            }
            else
            {
                // add new user
                Users.Add(user);
            }

            SaveUserData(new UserData { Users = Users.ToArray() });
        }

        private IList<User> Users
        {
            get { return _users ?? (_users = LoadUserData().Users.ToList()); }
        }

        private static void SaveUserData(UserData userData)
        {
            CloudBlob blob = GetBlobReference();

            // serialize users to XML
            var serializer = new XmlSerializer(typeof(UserData));
            using (var memoryStream = new MemoryStream())
            {
                serializer.Serialize(memoryStream, userData);
                memoryStream.Seek(0, SeekOrigin.Begin);  // reset stream to Beginning before upload

               // blob.DeleteIfExists();
                blob.Properties.ContentType = "text/xml";
                blob.UploadFromStream(memoryStream);
            }
        }

        private static UserData LoadUserData()
        {
            CloudBlob blob = GetBlobReference();
            if (!blob.Exists())
            {
                return new UserData {Users = new User[0]};
            }

            // download blob into memory
            var memoryStream = new MemoryStream();
            blob.DownloadToStream(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);

            // deserialize
            var serializer = new XmlSerializer(typeof(UserData));
            using (memoryStream)
            {
                var userData = (UserData)serializer.Deserialize(memoryStream);
                return userData;
            }
        }

        private static CloudBlob GetBlobReference()
        {
            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the blob client
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // create the blob container (if it doesn't exist yet)
            CloudBlobContainer container = blobClient.GetContainerReference(ContainerName);

            container.CreateIfNotExist();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            // access blob
            CloudBlob blob = container.GetBlobReference(BlobName);
            return blob;
        }
    }

    public static class CloudBlobExtensions
    {
        public static bool Exists(this CloudBlob blob)
        {
            try
            {
                if (blob is CloudBlockBlob)
                {
                    ((CloudBlockBlob) blob).FetchAttributes();
                    return true;
                }
                if (blob is CloudPageBlob)
                {
                    ((CloudPageBlob) blob).FetchAttributes();
                    return true;
                }
                try
                {
                    (blob).FetchAttributes();
                    return true;
                }
                catch (InvalidOperationException ex)
                {
                    if (ex.Message == "BlobType of the blob reference doesn't match BlobType of the blob")
                        return true;
                    throw;
                }

            }
            catch (StorageClientException e)
            {
                if (e.ErrorCode == StorageErrorCode.ResourceNotFound)
                {
                    return false;
                }
                throw;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace RecipeWebRole.DataAccess
{
    public class TableStorageRepository 
    {
        const string TableName = "recipe";
        private const string DefaultPartitionKey = "dk4";

        public IEnumerable<int> GetRecipeIds()
        {
            CloudTableClient tableClient = GetCloudTableClient();

            string[] ids;
            if (tableClient.DoesTableExist(TableName))
            {
                // Get the data service context
                TableServiceContext serviceContext = tableClient.GetDataServiceContext();

                // Specify a partition query
                CloudTableQuery<RecipeEntity> partitionQuery =
                    (from e in serviceContext.CreateQuery<RecipeEntity>(TableName)
                     where e.PartitionKey == DefaultPartitionKey
                     select e).AsTableServiceQuery<RecipeEntity>();

                var list = new List<string>();
                foreach (var recipeEntity in partitionQuery)
                {
                    list.Add(recipeEntity.RowKey);
                }
                ids = list.ToArray();
                //ids = partitionQuery.Select(entity => entity.RowKey).ToArray();

            }
            else
            {
                ids = new string[] {"12", "13", "14"};
            }

            return ids.Select(Int32.Parse);
        }

        public void AddRecipe(int id, string text)
        {
            var recipeEntity = new RecipeEntity(DefaultPartitionKey, id.ToString(CultureInfo.InvariantCulture)) {Data = text};
            
            CreateTableIfNotExist();

            CloudTableClient tableClient = GetCloudTableClient();

            // Get the data service context
            TableServiceContext serviceContext = tableClient.GetDataServiceContext();

            // Add the new recipe to the recipe table
            serviceContext.AddObject(TableName, recipeEntity);

            // Submit the operation to the table service
            serviceContext.SaveChangesWithRetries();
        }

        private void CreateTableIfNotExist()
        {
            CloudTableClient tableClient = GetCloudTableClient();

            tableClient.CreateTableIfNotExist(TableName);
        }

        //private CloudTableClient GetCloudTableClient()
        //{
        //    // Variante
        //    const string accountname = "devstoreaccount1";
        //    const string accountkey = "Eby8vdM02xNOcqFlqUwJPLlmEtlCDXJ1OUzFT50uSRZ6IFsuFq2UVErCz4I6tq/K1SZFPTOtr/KBHBeksoGMGw==";
        //    const string tableServiceAddress = "http://127.0.0.1:10002/devstoreaccount1";
        //    StorageCredentials creds = new StorageCredentialsAccountAndKey(accountname, accountkey);
        //    var tableClient = new CloudTableClient(tableServiceAddress, creds);
        //}

        private CloudTableClient GetCloudTableClient()
        {
            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting("StorageConnectionString"));

            // Create the table client
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            return tableClient;
        }


        public sealed class RecipeEntity : TableServiceEntity
        {
            public RecipeEntity(string partitionKey, string id) :
                base(partitionKey, id)
            {
            }

            public RecipeEntity()
            {
            }

            public string Data { get; set; }
        }
    }
}
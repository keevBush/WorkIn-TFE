using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace WorkInApi.Services
{
    public class StorageAzureManager
    {
        private CloudBlobContainer blobContainer;
        public StorageAzureManager(string ContainerName)
        {
            // Check if Container Name is null or empty  
            if (string.IsNullOrEmpty(ContainerName))
            {
                throw new ArgumentNullException("ContainerName", "Container Name can't be empty");
            }
            try
            {
                // Get azure table storage connection string.  
                string ConnectionString = "DefaultEndpointsProtocol=https;AccountName=workinapi;AccountKey=i0Gmy98TzZnv+bjeaXE91whsxlDJoT7/OrpcdNFarrgwD4hnJpQzJHbojj+mmyDlM6FC3ODcCkWsyDVm6VfJEA==;EndpointSuffix=core.windows.net";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);

                CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();
                blobContainer = cloudBlobClient.GetContainerReference(ContainerName);

                // Create the container and set the permission  
                if (blobContainer.CreateIfNotExistsAsync().GetAwaiter().GetResult())
                {
                    blobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                    );
                }
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
        }
        public async Task<string> UpladFile(string name, Stream FileToUpload, string contentType)
        {
            string AbsoluteUri;
            // Check HttpPostedFileBase is null or not  
            if (FileToUpload == null || FileToUpload.Length == 0)
                return null;
            try
            {
                string FileName = Path.GetFileName(name);
                CloudBlockBlob blockBlob;
                // Create a block blob  
                blockBlob = blobContainer.GetBlockBlobReference(FileName);
                // Set the object's content type  
                blockBlob.Properties.ContentType = contentType;
                // upload to blob  
                await blockBlob.UploadFromStreamAsync(FileToUpload);

                // get file uri  
                AbsoluteUri = blockBlob.Uri.AbsoluteUri;
            }
            catch (Exception ExceptionObj)
            {
                throw ExceptionObj;
            }
            return AbsoluteUri;
        }
    }
}

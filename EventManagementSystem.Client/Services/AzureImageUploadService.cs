using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Http;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Azure.Storage.Blobs.Models;
using System.IO;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.Extensions.Configuration;

namespace EventManagementSystem.Client.Services
{
    public class AzureImageUploadService
    {
        //private readonly string ConnectionString; 
        private readonly string Key;
        private readonly string AccountName;
        private readonly string ContainerName;
        private readonly CloudStorageAccount blobContainerClient;
        public AzureImageUploadService(IConfiguration config)
        {
            IConfiguration _config = config;

            Key = _config["AzureStorageForImages:BlobStorage:Key"].ToString();
            //ConnectionString = _config["AzureStorageForImages:BlobStorage:ConnectionString"].ToString();
            AccountName = _config["AzureStorageForImages:BlobStorage:AccountName"].ToString();
            ContainerName = _config["AzureStorageForImages:BlobStorage:ContainerName"].ToString();


            blobContainerClient = new CloudStorageAccount(new StorageCredentials(AccountName, Key),true);
        }

        internal async Task<bool> UploadFiles(List<IFormFile> imageFiles,string EventID)
        {
            bool IsSuccess = false;
            if (imageFiles.Count > 0)
            {
                var FilePathOnCloud = EventID+"\\";
                //var File = Directory.GetFiles(item.pa,string.Empty);
                var container = blobContainerClient.CreateCloudBlobClient().GetContainerReference(ContainerName);
                foreach (var item in imageFiles)
                {
                    var Blob = container.GetBlockBlobReference(FilePathOnCloud+item.FileName);
                    var File = item.OpenReadStream();
                    await Blob.UploadFromStreamAsync(File);
                    IsSuccess = true;
                }

                return IsSuccess;
            }
            else
                return IsSuccess;
        }

        internal async Task<bool> DeleteFile(string id, string EventID)
        {
            bool IsSuccess = false;
            try
            {
                if (!string.IsNullOrEmpty(id))
                {
                    var FilePathOnCloud = EventID + "\\"+id;
                    //var File = Directory.GetFiles(item.pa,string.Empty);
                    var container = blobContainerClient.CreateCloudBlobClient().GetContainerReference(ContainerName);

                    var Blob = container.GetBlockBlobReference(FilePathOnCloud);
                    //var File = item.OpenReadStream();
                    //await Blob.UploadFromStreamAsync(File);
                    IsSuccess = await Blob.DeleteIfExistsAsync();
                   
                    return IsSuccess;
                }
                else
                    return IsSuccess;


            }
            catch (Exception er)
            {
                return false;
            }
        }


        //public void Import(IFormFile filepond)
        //{
        //    const string accountName = "accountName";
        //    const string key = "key14881851";

        //    var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, key), true);
        //    var blobClient = storageAccount.CreateCloudBlobClient();
        //    var container = blobClient.GetContainerReference("mycontainer");
        //    await container.CreateIfNotExistsAsync();
        //    container.SetPermissionsAsync(new BlobContainerPermissions()
        //    {
        //        PublicAccess = BlobContainerPublicAccessType.Blob
        //    });

        //    var blob = container.GetBlockBlobReference("test.jpg");
        //    using (var stream = filepond.OpenReadStream())
        //    {
        //        await blob.UploadFromStreamAsync(stream);
        //    }



        //}




    }
}

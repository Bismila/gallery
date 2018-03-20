using System;
using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using Gallery.BAL.Interfaces;

namespace Gallery.BAL.Services
{
    public class BlobService // : IFileService
    {
        const string strorageAccName = "andersenimages";
        const string storageAccKey = "9r3RMB/0zxsoXq9nA+Pn8wz19ljReuQSWjuS+FU99TkR53K7d786cwPN+pfFGrEkxynGduwP5iSwyp9sdwHkPg==";

        public string UploadFile(Stream stream, string fileName, long userId)
        {
            string folder = "";
            var storageAccount = new CloudStorageAccount(new StorageCredentials(strorageAccName, storageAccKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudContainer = blobClient.GetContainerReference("imgs");

            cloudContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            CloudBlockBlob b;
            Uri path = null;
            if (userId > 0)
            {
                folder = userId.ToString() + @"/";

                b = cloudContainer.GetBlockBlobReference(folder + fileName);
                if (b.Exists())
                {
                    return "";
                }
                else
                {
                    b = cloudContainer.GetBlockBlobReference(folder + fileName);
                }
            }
            else
            {
                b = cloudContainer.GetBlockBlobReference(fileName);
            }
            b.UploadFromStream(stream);
            path = b.Uri;
            return path.ToString();
        }
        private static void DownLoadsBlobs(IEnumerable<IListBlobItem> blobs)
        {
            foreach (var blob in blobs)
            {
                if (blob is CloudBlockBlob blockBlob)
                {
                    blockBlob.DownloadToFile(blockBlob.Name, FileMode.Create);
                    Console.WriteLine(blockBlob.Name);
                }
                else if (blob is CloudBlobDirectory blobDirectory)
                {
                    Directory.CreateDirectory(blobDirectory.Prefix);
                    Console.WriteLine("Create directory " + blobDirectory.Prefix);
                    DownLoadsBlobs(blobDirectory.ListBlobs());
                }
            }
        }

        public void DeleteFile(string filePath, long userId)
        {
            string fileName = Path.GetFileName(filePath);

            var storageAccount = new CloudStorageAccount(new StorageCredentials(strorageAccName, storageAccKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer cloudContainer = blobClient.GetContainerReference("imgs");
            if (userId > 0 && !string.IsNullOrWhiteSpace(fileName))
            {
                var blockDir = cloudContainer.GetDirectoryReference(userId.ToString());
                var blobFile = blockDir.GetBlockBlobReference(fileName);
                blobFile.Delete();
            }
        }

        public void DeleteAllFile(long userId)
        {
            var storageAccount = new CloudStorageAccount(new StorageCredentials(strorageAccName, storageAccKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer cloudContainer = blobClient.GetContainerReference("imgs");

            cloudContainer.SetPermissions(new BlobContainerPermissions
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            var currentUserPath = cloudContainer.StorageUri.PrimaryUri + "/" + userId.ToString();

            var blobCurrentUser = cloudContainer.GetBlockBlobReference(currentUserPath);

            var allBlobs = cloudContainer.ListBlobs();

            foreach (IListBlobItem item in allBlobs)
            {
                if (item.GetType() == typeof(CloudBlobDirectory))
                {
                    CloudBlobDirectory directory = (CloudBlobDirectory)item;
                    if (directory.Prefix == userId.ToString() + @"/")
                    {
                        var listFiles = directory.ListBlobs();
                        foreach (var file in listFiles)
                        {
                            var currentFilePath = file.Uri;
                            DeleteFile(currentFilePath.ToString(), userId);
                        }
                        break;
                    }
                }
            }

        }

    }
}

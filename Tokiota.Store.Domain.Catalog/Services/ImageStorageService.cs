namespace Tokiota.Store.Domain.Catalog.Services
{
    using Microsoft.WindowsAzure.Storage;
    using Microsoft.WindowsAzure.Storage.Blob;
    using System.Configuration;
    using System.IO;

    internal class ImageStorageService : IImageStorageService
    {
        private const string AzureStorageConnectionStringKey = "StorageConnectionString";
        private readonly CloudStorageAccount storageAccount;

        public ImageStorageService()
        {
            this.storageAccount = CloudStorageAccount.Parse(ConfigurationManager.ConnectionStrings[AzureStorageConnectionStringKey].ConnectionString);
        }

        public string SaveImage(string fileName, Stream stream)
        {
            var blobName = Path.GetFileName(fileName);
            var container = this.GetOrCreateContainer();
            var blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.UploadFromStream(stream);

            return blockBlob.Uri.ToString();
        }

        public void DeleteImage(string imageUrl)
        {
            var blobName = Path.GetFileName(imageUrl);
            var container = this.GetOrCreateContainer();
            var blockBlob = container.GetBlockBlobReference(blobName);
            blockBlob.Delete();
        }

        private CloudBlobContainer GetOrCreateContainer()
        {
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference("images");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
            return container;
        }
    }
}

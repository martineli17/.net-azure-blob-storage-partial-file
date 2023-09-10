using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Text;

namespace Api
{
    public class BlobService
    {
        private readonly BlobServiceClient _blobClient;
        public BlobService(string connectionString)
        {
            _blobClient = new BlobServiceClient(connectionString);
        }

        public async Task UploadFileAsync(Stream file, string fileName, string containerName)
        {
            var containerClient = _blobClient.GetBlobContainerClient(containerName);
            await containerClient.CreateIfNotExistsAsync();

            await containerClient.UploadBlobAsync(fileName, file);
        }

        public async Task<string> GetPartialFileAsync(long offset, long length, string fileName, string containerName)
        {
            var httpRange = new HttpRange(offset, length);
            var containerClient = _blobClient.GetBlobContainerClient(containerName);
            var blobClient = containerClient.GetBlobClient(fileName);
            var contentFile = await blobClient.DownloadContentAsync(new BlobDownloadOptions()
            {
                Range = httpRange,
            });

            return Encoding.UTF8.GetString(contentFile.Value.Content);
        }
    }
}

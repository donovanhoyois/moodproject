using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MoodProject.Api.Configuration;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class FilesController
{
    private BlobServiceClient BlobServiceClient { get; set; }
    
    public FilesController(FileStorageConfiguration fileStorageConfiguration)
    {
        BlobServiceClient = new BlobServiceClient(
            new Uri("https://moodprojectstorage.blob.core.windows.net"),
            new StorageSharedKeyCredential(fileStorageConfiguration.AccountName, fileStorageConfiguration.AccountKey));
    }

    [HttpGet, ActionName("CheckConnection")]
    public async Task<bool> CheckConnection()
    {
        await BlobServiceClient.CreateBlobContainerAsync("test");
        return true;
    }

    [HttpPut, ActionName("Upload"), Consumes("application/octet-stream")]
    public async Task Upload(Stream stream)
    {
        stream.Close();
        //TODO: implementer et tester
    }
}
using Azure.Storage;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoodProject.Api.Configuration;
using MoodProject.Core.Models;

namespace MoodProject.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]/")]
[EnableCors]
public class FilesController
{
    private readonly MoodProjectContext DbContext;
    private BlobServiceClient BlobServiceClient { get; set; }
    private string RessourcesFolderName { get; init; }
    
    public FilesController(MoodProjectContext dbContext, FileStorageConfiguration fileStorageConfiguration)
    {
        DbContext = dbContext;
        BlobServiceClient = new BlobServiceClient(
            new Uri(fileStorageConfiguration.Url),
            new StorageSharedKeyCredential(fileStorageConfiguration.AccountName, fileStorageConfiguration.AccountKey));
        RessourcesFolderName = fileStorageConfiguration.ExternalRessourcesFolderName;
    }

    [HttpPut, ActionName("Upload")]
    public async Task<string> Upload(FileWithContent file)
    {
        var blobContainerClient = BlobServiceClient.GetBlobContainerClient(RessourcesFolderName);
        await blobContainerClient.CreateIfNotExistsAsync();
        var blobClient = blobContainerClient.GetBlobClient(file.Name);
        using (var stream = new MemoryStream(Convert.FromBase64String(file.Base64Content)))
        {
            await blobClient.UploadAsync(BinaryData.FromStream(stream), true);
        }

        DbContext.Add(new RessourceFile(int.Parse(file.ParentName), file.Name, blobClient.Uri));
        DbContext.SaveChanges();
        return blobClient.Uri.ToString();
    }
}
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
    private readonly ILogger<FilesController> Logger;
    private readonly MoodProjectContext DbContext;
    private BlobServiceClient BlobServiceClient { get; set; }
    private string RessourcesFolderName { get; init; }
    
    public FilesController(ILogger<FilesController> logger, MoodProjectContext dbContext, FileStorageConfiguration fileStorageConfiguration)
    {
        Logger = logger;
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
        var blobClient = blobContainerClient.GetBlobClient($"{file.ParentName}/{file.Name}");
        using (var stream = new MemoryStream(Convert.FromBase64String(file.Base64Content)))
        {
            await blobClient.UploadAsync(BinaryData.FromStream(stream), true);
        }

        DbContext.Add(new ResourceFile(int.Parse(file.ParentName), file.Name, blobClient.Uri));
        DbContext.SaveChanges();
        return blobClient.Uri.ToString();
    }

    [HttpGet, ActionName("Download")]
    public async Task<FileWithContent?> Download(int id)
    {
        var file = DbContext.ResourceFiles.FirstOrDefault(r => r.Id.Equals(id));
        if (file == null)
        {
            Logger.LogWarning("File with id {} is trying to be downloaded but is not found in database.", id);
            return null;
        }

        try
        {
            var blobContainerClient = BlobServiceClient.GetBlobContainerClient(RessourcesFolderName);
            var blobClient = blobContainerClient.GetBlobClient($"{file.ResourceId}/{file.Name}");
            var download = await blobClient.DownloadContentAsync();
            
            var base64Content = Convert.ToBase64String(download.Value.Content);
            return new FileWithContent(file.ResourceId.ToString(), file.Name, base64Content);
        }
        catch (Exception e)
        {
            Logger.LogError("Error while downloading file {} ({}) from Azure Blob Storage: {}", file.Id, file.Name, e);
        }
        return null;
    }
    
    [HttpDelete, ActionName("Delete")]
    public async Task<bool> Delete(int id)
    {
        var file = DbContext.ResourceFiles.FirstOrDefault(r => r.Id.Equals(id));
        if (file != null)
        {
            // Remove file from Azure Blob Storage
            var blobContainerClient = BlobServiceClient.GetBlobContainerClient(RessourcesFolderName);
            var blobClient = blobContainerClient.GetBlobClient($"{file.ResourceId}/{file.Name}");
            await blobClient.DeleteAsync(snapshotsOption: DeleteSnapshotsOption.IncludeSnapshots);
            
            // Remove from database
            DbContext.ResourceFiles.Remove(file);
        }

        return DbContext.SaveChanges() > 0;
    }
}
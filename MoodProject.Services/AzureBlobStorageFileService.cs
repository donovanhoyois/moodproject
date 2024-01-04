using MoodProject.Core;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class AzureBlobStorageFileService : IFileService
{
    private readonly IAppApi AppApi;
    
    public AzureBlobStorageFileService(IAppApi appApi)
    {
        AppApi = appApi;
    }

    public async Task<string> Upload(FileWithContent file)
    {
        file.Name = file.Name.Replace(" ", "_");
        return await AppApi.UploadFile(file);
    }
}
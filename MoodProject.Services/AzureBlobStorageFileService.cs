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
    public async Task Upload(Stream stream)
    {
        await AppApi.UploadFile(stream);
    }
}
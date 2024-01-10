using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class FileService : IFileService
{
    private readonly IAppApi AppApi;
    
    public FileService(IAppApi appApi)
    {
        AppApi = appApi;
    }

    public async Task<string> Upload(FileWithContent file)
    {
        file.Name = file.Name.Replace(" ", "_");
        return await AppApi.UploadFile(file);
    }

    public async Task<FileWithContent?> Download(int fileId)
    {
        return await AppApi.DownloadFile(fileId);
    }
}
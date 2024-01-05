using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IFileService
{
    public Task<string> Upload(FileWithContent file);
    public Task<FileWithContent?> Download(int fileId);
}
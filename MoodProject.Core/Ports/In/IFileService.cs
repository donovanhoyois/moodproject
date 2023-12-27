namespace MoodProject.Core.Ports.In;

public interface IFileService
{
    public Task Upload(Stream stream);
}
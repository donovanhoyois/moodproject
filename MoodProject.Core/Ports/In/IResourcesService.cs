using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IResourcesService
{
    public Task<OperationResult<IEnumerable<Resource>>> GetAll();
    public Task<OperationResult<Resource>> GetById(int id);
    public Task<OperationResult<Resource>> Create(Resource resource, List<FileWithContent> files);
}
using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class ResourcesService : IResourcesService
{
    private readonly IAppApi AppApi;
    private readonly IFileService FileService;
    
    public ResourcesService(IAppApi appApi, IFileService fileService)
    {
        AppApi = appApi;
        FileService = fileService;
    }

    public async Task<OperationResult<IEnumerable<Resource>>> GetAll()
    {
        var response = await AppApi.GetRessources();
        return response.Any()
            ? new OperationResult<IEnumerable<Resource>>(OperationResultType.Ok)
            {
                Content = response
            }
            : new OperationResult<IEnumerable<Resource>>(OperationResultType.Error)
            {
                Message = "Aucune ressource disponible."
            };
    }

    public async Task<OperationResult<Resource>> GetById(int id)
    {
        var response = await AppApi.GetRessource(id);
        return response != null
            ? new OperationResult<Resource>(OperationResultType.Ok)
            {
                Content = response
            }
            : new OperationResult<Resource>(OperationResultType.Error)
            {
                Message = "La ressource n'est pas disponible."
            };
    }

    public async Task<OperationResult<Resource>> Create(Resource resource, List<FileWithContent> files)
    {
        var createdRessource = await AppApi.CreateRessource(resource);
        var uploadTasks = new List<Task<string>>();
        foreach (var file in files)
        {
            file.ParentName = createdRessource.Id.ToString();
            uploadTasks.Add(FileService.Upload(file));
        }
        await Task.WhenAll(uploadTasks);
        
        createdRessource = await AppApi.GetRessource(createdRessource.Id);
        return createdRessource != null
            ? new OperationResult<Resource>(OperationResultType.Ok) { Content = createdRessource, Message = "La ressource a bien été créée"}
            : new OperationResult<Resource>(OperationResultType.Error);
    }
}
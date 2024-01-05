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
        var createdResource = await AppApi.CreateRessource(resource);
        var uploadTasks = new List<Task<string>>();
        foreach (var file in files)
        {
            file.ParentName = createdResource.Id.ToString();
            uploadTasks.Add(FileService.Upload(file));
        }
        await Task.WhenAll(uploadTasks);
        
        createdResource = await AppApi.GetRessource(createdResource.Id);
        return createdResource != null
            ? new OperationResult<Resource>(OperationResultType.Ok) { Content = createdResource, Message = "La ressource a bien été créée."}
            : new OperationResult<Resource>(OperationResultType.Error);
    }

    public async Task<OperationResult<bool>> Delete(int id)
    {
        var resource = await AppApi.GetRessource(id);
        if (resource != null)
        {
            List<Task<bool>> filesDeletions = new();
            foreach (var file in resource.Files)
            {
                filesDeletions.Add(AppApi.DeleteFile(file.Id));
            }

            await Task.WhenAll(filesDeletions);
            return await AppApi.DeleteResource(resource.Id)
                ? new OperationResult<bool>(OperationResultType.Ok) { Message = "La ressource a bien été supprimée."}
                : new OperationResult<bool>(OperationResultType.Error) { Message = "La ressource n'a pas pu être supprimée."};
        }
        return new OperationResult<bool>(OperationResultType.Error) { Message = "Une erreur est survenue lors de la suppression de la ressource."};
    }
}
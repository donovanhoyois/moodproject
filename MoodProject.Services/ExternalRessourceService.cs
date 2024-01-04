using MoodProject.Core.Enums;
using MoodProject.Core.Models;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;

namespace MoodProject.Services;

public class ExternalRessourceService : IExternalRessourceService
{
    private readonly IAppApi AppApi;
    private readonly IFileService FileService;
    
    public ExternalRessourceService(IAppApi appApi, IFileService fileService)
    {
        AppApi = appApi;
        FileService = fileService;
    }
    public async Task<OperationResult<Ressource>> Create(Ressource ressource, List<FileWithContent> files)
    {
        var createdRessource = await AppApi.CreateRessource(ressource);
        var uploadTasks = new List<Task<string>>();
        foreach (var file in files)
        {
            file.ParentName = createdRessource.Id.ToString();
            uploadTasks.Add(FileService.Upload(file));
        }
        await Task.WhenAll(uploadTasks);
        
        createdRessource = await AppApi.GetRessource(createdRessource.Id);
        return createdRessource != null
            ? new OperationResult<Ressource>(OperationResultType.Ok) { Content = createdRessource, Message = "La ressource a bien été créée"}
            : new OperationResult<Ressource>(OperationResultType.Error);
    }
}
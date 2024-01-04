using MoodProject.Core.Models;

namespace MoodProject.Core.Ports.In;

public interface IExternalRessourceService
{
    public Task<OperationResult<Ressource>> Create(Ressource ressource, List<FileWithContent> files);
}
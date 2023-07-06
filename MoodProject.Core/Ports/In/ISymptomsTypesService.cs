namespace MoodProject.Core.Ports.In;

public interface ISymptomsTypesService
{
    public Task<IEnumerable<SymptomType>> GetAll();
}
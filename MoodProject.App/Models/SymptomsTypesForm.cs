using MoodProject.Core;
using MoodProject.Core.Models;

namespace MoodProject.App.Models;

class SymptomsTypesForm
{
    public List<CheckableSymptomType> symptomsTypesCheckboxes = new();

    public SymptomsTypesForm(List<SymptomType> types)
    {
        foreach (var type in types)
        {
            symptomsTypesCheckboxes.Add(new CheckableSymptomType()
            {
                Id = type.Id,
                IsChecked = false,
                Name = type.Name
            });
        }
    }
}
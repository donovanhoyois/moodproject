using MoodProject.Core.Models;

namespace MoodProject.App.Models;

class CheckableSymptomType : SymptomType
{
    public bool IsChecked { get; set; }
}
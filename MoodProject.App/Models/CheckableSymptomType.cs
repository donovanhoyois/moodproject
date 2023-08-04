using MoodProject.Core;

namespace MoodProject.App.Models;

class CheckableSymptomType : SymptomType
{
    public bool IsChecked { get; set; }
}
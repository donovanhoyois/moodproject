using Microsoft.AspNetCore.Components.Forms;
using MoodProject.Core.Models;

namespace MoodProject.App.Models;

public class RessourceFileToUpload
{
    public IBrowserFile BrowserFile { get; init; }
    public RessourceFileToUpload(IBrowserFile browserFile)
    {
        BrowserFile = browserFile;
    }
}
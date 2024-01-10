using Microsoft.AspNetCore.Components.Forms;

namespace MoodProject.App.Models;

public class RessourceFileToUpload
{
    public IBrowserFile BrowserFile { get; init; }
    public RessourceFileToUpload(IBrowserFile browserFile)
    {
        BrowserFile = browserFile;
    }
}
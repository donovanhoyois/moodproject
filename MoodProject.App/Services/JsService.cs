using Microsoft.JSInterop;

namespace MoodProject.App.Services;

public class JsService
{
    private IJSRuntime JsRuntime { get; }
    private IJSObjectReference? JsObjectRef { get; set; }

    public JsService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task Execute(string functionName, string paramArgs)
    {
        JsObjectRef ??= await JsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/customs.js");
        if (JsObjectRef != null)
        {
            await JsObjectRef.InvokeVoidAsync(functionName, paramArgs);
        }
        
    }
}
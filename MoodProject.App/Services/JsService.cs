using Microsoft.JSInterop;

namespace MoodProject.App.Services;

public class JsService
{
    private IJSRuntime JsRuntime { get; }

    public JsService(IJSRuntime jsRuntime)
    {
        JsRuntime = jsRuntime;
    }

    public async Task Execute(string functionName, params object?[] paramArgs)
    {
        await JsRuntime.InvokeVoidAsync(functionName, paramArgs);
    }

    public async Task<T?> Execute<T>(string functionName) where T : new()
    {
        return await JsRuntime.InvokeAsync<T>(functionName);
    }
}
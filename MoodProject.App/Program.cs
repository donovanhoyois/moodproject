using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoodProject.App;
using MoodProject.Core.Ports.In;
using MoodProject.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44337/") });

// Custom
builder.Services.AddSingleton<ISymptomsTypesService, SymptomsTypesService>();
builder.Services.AddSingleton<ISymptomsFormService, SymptomsFormService>();

await builder.Build().RunAsync();

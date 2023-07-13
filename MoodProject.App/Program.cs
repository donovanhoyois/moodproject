using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoodProject.App;
using MoodProject.Core.Ports.In;
using MoodProject.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient { BaseAddress = new Uri("https://localhost:44337/api/") });

// Custom
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    //options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
});
//builder.Services.AddSingleton<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddSingleton<ISymptomsTypesService, SymptomsTypesService>();
builder.Services.AddSingleton<ISymptomsFormService, SymptomsFormService>();

await builder.Build().RunAsync();

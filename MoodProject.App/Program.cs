using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoodProject.App;
using MoodProject.Core;
using MoodProject.Core.Configuration;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;
using MoodProject.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddSingleton(sp => new HttpClient());

// Auth0
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    //options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
});
builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(CustomAccountFactory));

// Configuration
builder.Services.AddSingleton(provider =>
{
    var config = provider.GetService<IConfiguration>();
    return config.GetSection("Api").Get<ApiConfiguration>();
});

// Services
builder.Services.AddSingleton<IAppApi, AppApi>();
builder.Services.AddSingleton<ISymptomsTypesService, SymptomsTypesService>();
builder.Services.AddSingleton<ISymptomsService, SymptomsService>();
builder.Services.AddSingleton<IQuizzService, QuizzService>();
builder.Services.AddSingleton<IChatRoomsService, ChatRoomsService>();

await builder.Build().RunAsync();

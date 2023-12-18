using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MoodProject.App;
using MoodProject.App.Configuration;
using MoodProject.App.Services;
using MoodProject.Core.Configuration;
using MoodProject.Core.Ports.In;
using MoodProject.Core.Ports.Out;
using MoodProject.Services;
using MoodProject.Services.Configuration;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient<HttpClient>(sp => new HttpClient());

// Auth0
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
    //options.ProviderOptions.AdditionalProviderParameters.Add("audience", builder.Configuration["Auth0:Audience"]);
});
builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(CustomAccountFactory));

// Configuration
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Api").Get<ApiConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Cache").Get<CacheConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Notifications").Get<NotificationsConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Quizz").Get<QuizzConfiguration>());


// External Services
builder.Services.AddBlazoredLocalStorageAsSingleton();

// Services
builder.Services.AddSingleton<IAppApi, AppApi>();
builder.Services.AddSingleton<IApiAuthService, ApiAuthService>();
builder.Services.AddSingleton<ISymptomsTypesService, SymptomsTypesService>();
builder.Services.AddSingleton<ISymptomsService, SymptomsService>();
builder.Services.AddSingleton<IQuizzService, QuizzService>();
builder.Services.AddSingleton<IChatRoomsService, ChatRoomsService>();
builder.Services.AddSingleton<IUsersService, UsersService>();
builder.Services.AddSingleton<IMedicationService, MedicationService>();
builder.Services.AddSingleton<INotificationService, NotificationService>();

// Blazor-specific services
builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddScoped<JsService>();
builder.Services.AddScoped<NotificationClient>();

await builder.Build().RunAsync();

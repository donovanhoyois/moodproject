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

// HttpClient
builder.Services.AddScoped<HttpClient>(sp => new HttpClient());

// Auth0
builder.Services.AddOidcAuthentication(options =>
{
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.ResponseType = "code";
});
builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(CustomAccountFactory));

// Configuration
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Api").Get<ApiConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Cache").Get<CacheConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Notifications").Get<NotificationsConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Quizz").Get<QuizzConfiguration>());
builder.Services.AddSingleton(provider => provider.GetService<IConfiguration>().GetSection("Auth0").Get<AuthConfiguration>());

// Services
builder.Services.AddScoped<IAppApi, AppApi>();
builder.Services.AddScoped<IApiAuthService, ApiAuthService>();
builder.Services.AddScoped<ISymptomsTypesService, SymptomsTypesService>();
builder.Services.AddScoped<ISymptomsService, SymptomsService>();
builder.Services.AddScoped<IQuizzService, QuizzService>();
builder.Services.AddScoped<IQuizzGenerator, QuizzGenerator>();
builder.Services.AddScoped<IHealthService, HealthService>();
builder.Services.AddScoped<IChatRoomsService, ChatRoomsService>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IMedicationService, MedicationService>();
builder.Services.AddScoped<INotificationService, NotificationService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IResourcesService, ResourcesService>();

// Blazor-specific services
builder.Services.AddScoped<IdentityService>();
builder.Services.AddScoped<CacheService>();
builder.Services.AddScoped<JsService>();
builder.Services.AddScoped<NotificationClient>();

// External Services
builder.Services.AddBlazoredLocalStorageAsSingleton();

await builder.Build().RunAsync();

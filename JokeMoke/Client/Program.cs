global using JokeMoke.Client.Services.EmailService;
global using JokeMoke.Client.Services.ProfileService;
global using JokeMoke.Client.Services.UserService;
global using JokeMoke.Client.Services.JokeService;
global using JokeMoke.Client.Services.CommentService;
global using JokeMoke.Client.Services.StatService;
global using JokeMoke.Shared;
using JokeMoke.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Radzen;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri($"{builder.HostEnvironment.BaseAddress}api/") });

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IJokerService, JokeService>();
builder.Services.AddScoped<ICommentService, CommentService>();
builder.Services.AddScoped<IStatService, StatService>();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
await builder.Build().RunAsync();

using BlazorBookManagement;
using BlazorBookManagement.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? "https://localhost:7102"));
builder.Services.AddScoped<BooksService>();

await builder.Build().RunAsync();

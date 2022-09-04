using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TangyWebClient;
using TangyWebClient.Service;
using TangyWebClient.Service.IService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration.GetValue<string>("BaseApiUrl")) });
builder.Services.AddScoped<IProductService, ProductService>();



await builder.Build().RunAsync();

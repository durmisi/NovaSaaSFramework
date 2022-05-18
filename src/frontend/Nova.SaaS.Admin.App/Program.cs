using Microsoft.Extensions.FileProviders;
using Nacos.AspNetCore.V2;
using Westwind.AspNetCore.LiveReload;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var environment = builder.Environment;

// nacos server v1.x or v2.x
builder.Services.AddNacosAspNet(configuration);

// Add services to the container.
var mvcBuilder = builder.Services
    .AddControllersWithViews();

if (environment.IsDevelopment())
{
    mvcBuilder.AddRazorRuntimeCompilation();

    builder.Services.AddLiveReload(config =>
    {
        // optional - use config instead
        config.LiveReloadEnabled = true;
        config.ClientFileExtensions = ".cshtml,.css,.js,.htm,.html,.ts,.razor,.custom";
        //config.WebSocketUrl = "ws://localhost:9700/admin/__livereload";
        //config.LiveReloadScriptUrl = "/__livereloadscript";

        //config.FolderToMonitor = "~/";
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseLiveReload();
}

//app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), @"client/build")),
});

app.UseRouting();

app.UseAuthorization();

app.MapDefaultControllerRoute();

app.Run();

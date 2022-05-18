using Microsoft.Extensions.FileProviders;
using Nacos.AspNetCore.V2;

var builder = WebApplication.CreateBuilder(args);

// nacos server v1.x or v2.x
builder.Services.AddNacosAspNet(builder.Configuration);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
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

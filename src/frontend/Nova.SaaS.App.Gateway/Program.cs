using Nacos.V2.DependencyInjection;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddNacosV2Naming(x =>
{
    x.ServerAddresses = new System.Collections.Generic.List<string> {
        "http://nacos:8848/"
    };

    x.EndPoint = "";
    x.Namespace = "yarp";
    x.UserName = "nacos";
    x.Password = "nacos";

    // swich to use http or rpc
    x.NamingUseRpc = true;
});


var proxyBuilder = builder.Services.AddReverseProxy();

proxyBuilder.AddNacosServiceDiscovery(
    groupNames: "DEFAULT_GROUP",
    percount: 100,
    enableAutoRefreshService: true,
    autoRefreshPeriod: 30);

var app = builder.Build();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/yarp", (IProxyConfigProvider provider) =>
    {
        var res = provider.GetConfig();
        return Results.Ok(res);
    });

    endpoints.MapReverseProxy();

    endpoints.Map("/{**catch-all}", async httpContext =>
    {
        if (httpContext.Request.Path.HasValue)
        {
            if (httpContext.Request.Path != "/" || httpContext.Request.Path.Value.Contains("livereload"))
            {
                return;
            }
        }

        httpContext.Response.Redirect("http://localhost:9700/admin");
    });
});

app.Run();
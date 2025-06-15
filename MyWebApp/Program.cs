using FastEndpoints;
using Model.Models;
using Repository;

var bld = WebApplication.CreateBuilder(args);
bld.Services.AddFastEndpoints();
bld.Services.AddSingleton<OaDbContext>();
bld.Services.AddSingleton<RepositoryManager>();
//bld.Services.AddSingleton<ILogger, MyWebApp.Logger>();

var app = bld.Build();
app.UseFastEndpoints();
app.Run();
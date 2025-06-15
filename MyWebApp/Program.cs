using Contracts;
using FastEndpoints;
using FastEndpoints.Swagger;
using Model.Models;
using Repository;
using Repository.S3;

var bld = WebApplication.CreateBuilder(args);
bld.Services.AddFastEndpoints();
//bld.Services.AddEndpointsApiExplorer();
bld.Services.AddSwaggerDocument();
bld.Services.AddScoped<OaDbContext>();
bld.Services.AddScoped<RepositoryManager>();
bld.Services.AddScoped<ITestImageRepository, TestImageMockRepository>();
//bld.Services.AddSingleton<ILogger, MyWebApp.Logger>();

var app = bld.Build();
app.UseSwaggerUi();
app.UseFastEndpoints()
    .UseSwaggerGen(); //add this

app.Run();

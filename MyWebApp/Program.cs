using Contracts;
using FastEndpoints;
using Model.Models;
using Repository;
using Repository.S3;

var bld = WebApplication.CreateBuilder(args);
bld.Services.AddFastEndpoints();
bld.Services.AddEndpointsApiExplorer();
bld.Services.AddSwaggerGen();
bld.Services.AddScoped<OaDbContext>();
bld.Services.AddScoped<RepositoryManager>();
bld.Services.AddTestImageRepository(bld.Configuration);
//bld.Services.AddSingleton<ILogger, MyWebApp.Logger>();

var app = bld.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.UseFastEndpoints();
app.Run();

using System.Text;
using DataTransferObjects.Requests;
using DataTransferObjects.Responses;
using FastEndpoints;
using FastEndpoints.Testing;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MyWebApp.Endpoints;
using Repository.S3;
using Repository;
using Model.Models;
using Contracts;

namespace MyWebApp.Tests;

public class EndpointTests
{
    [Fact]
    public async Task PostTest_Creates_Test()
    {
        var repo = TestHelper.CreateRepositoryManager(out var ctx);
        var ep = Factory.Create<PostTest>(ctx =>
        {
            ctx.AddTestServices(s => s.AddSingleton(repo));
        });

        await ep.HandleAsync(default);

        Assert.Single(ctx.Tests);
        Assert.Equal(StatusCodes.Status201Created, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task GetTests_Returns_List()
    {
        var repo = TestHelper.CreateRepositoryManager(out var ctx);
        ctx.Tests.Add(new Test { Name = "T", Tags = new List<Tag>() });
        await ctx.SaveChangesAsync();
        var ep = Factory.Create<GetTests>(ctxBuilder =>
        {
            ctxBuilder.AddTestServices(s => s.AddSingleton(repo));
        });

        await ep.HandleAsync(default);

        Assert.Equal(StatusCodes.Status200OK, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task GetTests_NoData_Returns_NotFound()
    {
        var repo = TestHelper.CreateRepositoryManager(out _);
        var ep = Factory.Create<GetTests>(ctxBuilder =>
        {
            ctxBuilder.AddTestServices(s => s.AddSingleton(repo));
        });

        await ep.HandleAsync(default);

        Assert.Equal(StatusCodes.Status404NotFound, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task GetTestById_Returns_Test()
    {
        var repo = TestHelper.CreateRepositoryManager(out var ctx);
        var test = new Test { Name = "T" };
        ctx.Tests.Add(test);
        await ctx.SaveChangesAsync();

        var ep = Factory.Create<GetTestById>(c => c.AddTestServices(s => s.AddSingleton(repo)));

        await ep.HandleAsync(new GetTestByIdRequest { Id = test.Id }, default);

        Assert.Equal(StatusCodes.Status200OK, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task GetTestById_NotFound()
    {
        var repo = TestHelper.CreateRepositoryManager(out _);
        var ep = Factory.Create<GetTestById>(c => c.AddTestServices(s => s.AddSingleton(repo)));

        await ep.HandleAsync(new GetTestByIdRequest { Id = Guid.NewGuid() }, default);

        Assert.Equal(StatusCodes.Status404NotFound, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task PutTest_Updates_Test()
    {
        var repo = TestHelper.CreateRepositoryManager(out var ctx);
        var tag = new Tag { Name = "tag", Category = "cat", CategoryNavigation = new Category { Name = "cat" } };
        ctx.Tags.Add(tag);
        var test = new Test { Name = "T", Tags = new List<Tag> { tag } };
        ctx.Tests.Add(test);
        await ctx.SaveChangesAsync();

        var ep = Factory.Create<PutTest>(c => c.AddTestServices(s => s.AddSingleton(repo)));
        var req = new PutTestRequest
        {
            Id = test.Id,
            Name = "New",
            Description = "Desc",
            Content = "Content",
            Tags = new List<PutTestRequest.TagDTO> { new() { TagName = tag.Name, CategoryName = tag.Category } }
        };

        await ep.HandleAsync(req, default);

        Assert.Equal(StatusCodes.Status204NoContent, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task PutTest_TagMissing_Returns_NotFound()
    {
        var repo = TestHelper.CreateRepositoryManager(out var ctx);
        var test = new Test { Name = "T" };
        ctx.Tests.Add(test);
        await ctx.SaveChangesAsync();

        var ep = Factory.Create<PutTest>(c => c.AddTestServices(s => s.AddSingleton(repo)));
        var req = new PutTestRequest
        {
            Id = test.Id,
            Name = "New",
            Description = "Desc",
            Content = "Content",
            Tags = new List<PutTestRequest.TagDTO> { new() { TagName = "X", CategoryName = "Y" } }
        };

        await ep.HandleAsync(req, default);

        Assert.Equal(StatusCodes.Status404NotFound, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task GetImage_Returns_File()
    {
        var repoManager = TestHelper.CreateRepositoryManager(out _);
        var imgRepo = TestHelper.CreateImageRepository();
        var id = Guid.NewGuid();
        await imgRepo.UploadImageAsync(id, "img.png", new MemoryStream(Encoding.UTF8.GetBytes("abc")), "text/plain");

        var ep = Factory.Create<GetImage>(c =>
        {
            c.AddTestServices(s =>
            {
                s.AddSingleton(repoManager);
                s.AddSingleton<ITestImageRepository>(imgRepo);
            });
        });
        await ep.HandleAsync(new GetImageRequest { TestId = id, ImageName = "img.png" }, default);

        Assert.Equal(StatusCodes.Status200OK, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task GetImage_NotFound()
    {
        var repoManager = TestHelper.CreateRepositoryManager(out _);
        var imgRepo = TestHelper.CreateImageRepository();
        var ep = Factory.Create<GetImage>(c =>
        {
            c.AddTestServices(s =>
            {
                s.AddSingleton(repoManager);
                s.AddSingleton<ITestImageRepository>(imgRepo);
            });
        });

        await ep.HandleAsync(new GetImageRequest { TestId = Guid.NewGuid(), ImageName = "none" }, default);

        Assert.Equal(StatusCodes.Status404NotFound, ep.HttpContext.Response.StatusCode);
    }

    [Fact]
    public async Task PostImage_Uploads_File()
    {
        var repoManager = TestHelper.CreateRepositoryManager(out _);
        var imgRepo = TestHelper.CreateImageRepository();
        var ep = Factory.Create<PostImage>(c =>
        {
            c.AddTestServices(s =>
            {
                s.AddSingleton(repoManager);
                s.AddSingleton<ITestImageRepository>(imgRepo);
            });
        });
        var id = Guid.NewGuid();
        var data = new MemoryStream(Encoding.UTF8.GetBytes("abc"));
        var formFile = new FormFile(data, 0, data.Length, "image", "img.png") { Headers = new HeaderDictionary(), ContentType = "text/plain" };
        var req = new PostImageRequest { TestId = id, ImageName = "img.png", Image = formFile };

        await ep.HandleAsync(req, default);

        var downloaded = await imgRepo.DownloadTestImageAsync(id, "img.png");
        Assert.NotNull(downloaded.data);
        Assert.Equal(StatusCodes.Status200OK, ep.HttpContext.Response.StatusCode);
    }
}

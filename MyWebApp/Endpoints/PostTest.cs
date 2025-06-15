using FastEndpoints;
using Model.Models;
using Repository;

namespace MyWebApp.Endpoints;

public class PostTest : EndpointWithoutRequest<Guid>
{
    public required RepositoryManager RepositoryManager { get; set; }

    public override void Configure()
    {
        Post("/api/test");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        Test test = new Test
        {
            Name = "NONAME",
            Description = "Test description",
            Content = "Test content",
            Tags = new List<Tag>()
        };
        RepositoryManager.Test.Create(test);
        await RepositoryManager.SaveAsync();
        await SendAsync(test.Id, 201);
    }
}

using FastEndpoints;
using Services.Test;
using Model.Models;

namespace MyWebApp.Endpoints;

public class PostTest : EndpointWithoutRequest<Guid>
{
    public required TestService TestService { get; set; }

    public override void Configure()
    {
        Post("/api/test");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a sample test";
            s.Description = "Creates a test with placeholder data and returns its identifier.";
            s.Response<Guid>(201, "Identifier of the new test");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var test = await TestService.CreateSampleTestAsync(ct);
        await SendAsync(test.Id, 201);
    }
}

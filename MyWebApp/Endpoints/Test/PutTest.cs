using DataTransferObjects.Requests;
using FastEndpoints;
using Services.Test;
using Model.Models;

namespace MyWebApp.Endpoints;

public class PutTest : Endpoint<PutTestRequest>
{
    public required TestService TestService { get; set; }

    public override void Configure()
    {
        Put("/api/test/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Update a test";
            s.Description = "Updates an existing test with the provided data.";
            s.Response(204, "Test updated successfully");
            s.Response(404, "Test or tag not found");
        });
    }

    public override async Task HandleAsync(PutTestRequest req, CancellationToken ct)
    {
        var result = await TestService.UpdateTestAsync(req, ct);

        if(result == TestService.UpdateResult.TestNotFound)
        {
            await SendNotFoundAsync();
            return;
        }

        if(result == TestService.UpdateResult.TagNotFound)
        {
            await SendAsync("Tag not found", statusCode: 404);
            return;
        }

        await SendNoContentAsync();
    }
}

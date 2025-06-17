using FastEndpoints;
using Services.Category;

namespace MyWebApp.Endpoints;

public class DeleteTag : EndpointWithoutRequest
{
    public required CategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Delete("/api/category/{CategoryName}/tag/{TagName}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Delete tag";
            s.Description = "Deletes a tag from the specified category";
            s.Response(204, "Tag deleted");
            s.Response(404, "Tag not found");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var categoryName = Route<string>("CategoryName");
        var tagName = Route<string>("TagName");
        var deleted = await CategoryService.DeleteTagAsync(categoryName, tagName, ct);
        if (!deleted)
        {
            await SendNotFoundAsync();
            return;
        }
        await SendNoContentAsync();
    }
}

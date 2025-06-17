using FastEndpoints;
using Services.Category;

namespace MyWebApp.Endpoints;

public class DeleteCategory : EndpointWithoutRequest
{
    public required CategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Delete("/api/category/{Name}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Delete a category";
            s.Description = "Deletes the specified category";
            s.Response(204, "Category deleted");
            s.Response(404, "Category not found");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var name = Route<string>("Name");
        var deleted = await CategoryService.DeleteCategoryAsync(name, ct);
        if (!deleted)
        {
            await SendNotFoundAsync();
            return;
        }
        await SendNoContentAsync();
    }
}

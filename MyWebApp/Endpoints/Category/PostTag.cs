using DataTransferObjects.Requests;
using FastEndpoints;
using Services.Category;

namespace MyWebApp.Endpoints;

public class PostTag : Endpoint<TagRequest>
{
    public required CategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Post("/api/category/{CategoryName}/tag");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create tag";
            s.Description = "Adds a tag to the specified category";
            s.Response(201, "Tag created");
            s.Response(404, "Category not found");
        });
    }

    public override async Task HandleAsync(TagRequest req, CancellationToken ct)
    {
        await CategoryService.CreateTagAsync(req.CategoryName, req.Name, ct);
        await SendAsync("", 201);
    }
}

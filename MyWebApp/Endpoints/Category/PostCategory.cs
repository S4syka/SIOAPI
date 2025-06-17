using DataTransferObjects.Requests;
using FastEndpoints;
using Services.Category;

namespace MyWebApp.Endpoints;

public class PostCategory : Endpoint<CategoryRequest>
{
    public required CategoryService CategoryService { get; set; }

    public override void Configure()
    {
        Post("/api/category");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Create a new category";
            s.Description = "Creates a category with the provided name";
            s.Response(201, "Category created");
        });
    }

    public override async Task HandleAsync(CategoryRequest req, CancellationToken ct)
    {
        await CategoryService.CreateCategoryAsync(req.Name, ct);
        await SendAsync("", 201);
    }
}

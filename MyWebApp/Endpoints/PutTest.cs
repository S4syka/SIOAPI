using DataTransferObjects.Requests;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Repository;

namespace MyWebApp.Endpoints;

public class PutTest : Endpoint<PutTestRequest>
{
    public required RepositoryManager RepositoryManager { get; set; }

    public override void Configure()
    {
        Put("/api/test/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(PutTestRequest req, CancellationToken ct)
    {
        Test? test = await RepositoryManager
            .Test.FindByCondition(t => t.Id.Equals(req.Id), true)
            .Include(t => t.Tags)
            .SingleOrDefaultAsync(ct);

        if (test == null)
        {
            await SendNotFoundAsync();
            return;
        }

        List<Tag> tags = new List<Tag>();

        foreach (var tag in req.Tags)
        {
            Tag? existingTag = await RepositoryManager.Tag.GetTagAsync(tag.TagName, tag.CategoryName, true);
            if(existingTag is null)
            {
                await SendAsync("Tag not found", statusCode: 404);
                return;
            }

            tags.Add(existingTag);
        }

        test.Name = req.Name;
        test.Description = req.Description;
        test.Content = req.Content;
        test.Tags.Clear();
        test.Tags = tags;
        await RepositoryManager.SaveAsync();
        await SendAsync(StatusCodes.Status204NoContent);
    }
}

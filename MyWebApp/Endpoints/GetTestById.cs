using FastEndpoints;
using DataTransferObjects;
using DataTransferObjects.Responses;
using DataTransferObjects.Requests;
using Repository;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace MyWebApp.Endpoints;

public class GetTestById : Endpoint<GetTestByIdRequest, GetTestByIdResponse>
{
    public required RepositoryManager RepositoryManager { get; set; }

    public override void Configure()
    {
        Get("/api/test/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetTestByIdRequest req, CancellationToken ct)
    {
        Test? test =  RepositoryManager.Test.FindAll(false).Include(t => t.Tags).Where(x => x.Id == req.Id).SingleOrDefault();

        if(test == null)
        {
            await SendNotFoundAsync();
        }
        else
        {
            await SendAsync(MapTestToResponse(test));
        }
    }

    private GetTestByIdResponse MapTestToResponse(Test test)
    {
        GetTestByIdResponse res = new GetTestByIdResponse
        {
            Id = test.Id,
            Name = test.Name,
            Description = test.Description ?? "",
            Content = test.Content ?? "",
            Tags = new Dictionary<string, List<string>>()
        };
    
        List<Tag> tags = test.Tags.ToList();

        foreach(var tag in tags)
        {
            if (!res.Tags.ContainsKey(tag.Category))
            {
                res.Tags.Add(tag.Category, new List<string>());
            }

            res.Tags[tag.Category].Add(tag.Name);
        }   

        return res;
    }
}

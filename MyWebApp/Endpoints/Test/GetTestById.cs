using FastEndpoints;
using DataTransferObjects;
using DataTransferObjects.Responses;
using DataTransferObjects.Requests;
using Services.Test;
using Model.Models;

namespace MyWebApp.Endpoints;

public class GetTestById : Endpoint<GetTestByIdRequest, GetTestByIdResponse>
{
    public required TestService TestService { get; set; }

    public override void Configure()
    {
        Get("/api/test/{Id}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get a single test";
            s.Description = "Returns detailed information about a test by its identifier.";
            s.Response<GetTestByIdResponse>(200, "The requested test");
            s.Response(404, "Test not found");
        });
    }

    public override async Task HandleAsync(GetTestByIdRequest req, CancellationToken ct)
    {
        var test = TestService.GetTestById(req.Id);

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

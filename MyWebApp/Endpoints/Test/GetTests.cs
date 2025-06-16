using FastEndpoints;
using Services.Test;
using DataTransferObjects.Requests;
using DataTransferObjects.Responses;
using Microsoft.EntityFrameworkCore;
using Model.Models;

namespace MyWebApp.Endpoints;

public class GetTests : EndpointWithoutRequest<GetTestsResponse>
{
    public required TestService TestService { get; set; }

    public override void Configure()
    {
        Get("/api/test/list");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Get all tests";
            s.Description = "Returns a list with basic information about every test.";
            s.Response<GetTestsResponse>(200, "List of tests");
            s.Response(404, "No tests were found");
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var tests = (await TestService.GetAllTestsAsync(ct)).ToList();

        if(tests == null || tests.Count == 0)
        {
            await SendNotFoundAsync();
        }
        else
        {
            GetTestsResponse res = new GetTestsResponse
            {
                Tests = MapTestsToTestElements(tests).ToList()
            };

            await SendAsync(res);
        }
    }

    private IEnumerable<GetTestsResponse.TestElement> MapTestsToTestElements(IEnumerable<Test> tests)
    {
        foreach (var test in tests)
        {
            yield return MapTestToTestElement(test);
        }
    }

    private GetTestsResponse.TestElement MapTestToTestElement(Test test)
    {
        GetTestsResponse.TestElement res = new GetTestsResponse.TestElement
        {
            Id = test.Id,
            Name = test.Name,
            Description = test.Description ?? "",
            Tags = new Dictionary<string, List<string>>()
        };

        List<Tag> tags = test.Tags.ToList();

        foreach (var tag in tags)
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

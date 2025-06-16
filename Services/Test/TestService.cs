using DataTransferObjects.Requests;
using Microsoft.EntityFrameworkCore;
using Model.Models;
using Repository;

namespace Services.Test;

public class TestService
{
    private readonly RepositoryManager _repositoryManager;

    public TestService(RepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<Model.Models.Test>> GetAllTestsAsync(CancellationToken ct)
    {
        return await _repositoryManager.Test
            .FindAll(false)
            .Include(t => t.Tags)
            .ToListAsync(ct);
    }

    public Model.Models.Test? GetTestById(Guid id)
    {
        return _repositoryManager.Test
            .FindAll(false)
            .Include(t => t.Tags)
            .Where(t => t.Id == id)
            .SingleOrDefault();
    }

    public async Task<Model.Models.Test> CreateSampleTestAsync(CancellationToken ct)
    {
        var test = new Model.Models.Test
        {
            Name = "NONAME",
            Description = "Test description",
            Content = "Test content",
            Tags = new List<Tag>()
        };

        _repositoryManager.Test.Create(test);
        await _repositoryManager.SaveAsync();
        return test;
    }

    public enum UpdateResult
    {
        Success,
        TestNotFound,
        TagNotFound
    }

    public async Task<UpdateResult> UpdateTestAsync(PutTestRequest req, CancellationToken ct)
    {
        var test = await _repositoryManager
            .Test.FindByCondition(t => t.Id.Equals(req.Id), true)
            .Include(t => t.Tags)
            .SingleOrDefaultAsync(ct);

        if (test == null)
        {
            return UpdateResult.TestNotFound;
        }

        var tags = new List<Tag>();
        foreach (var tag in req.Tags)
        {
            var existingTag = await _repositoryManager.Tag.GetTagAsync(tag.TagName, tag.CategoryName, true);
            if (existingTag is null)
            {
                return UpdateResult.TagNotFound;
            }
            tags.Add(existingTag);
        }

        test.Name = req.Name;
        test.Description = req.Description;
        test.Content = req.Content;
        test.Tags.Clear();
        test.Tags = tags;
        await _repositoryManager.SaveAsync();
        return UpdateResult.Success;
    }
}

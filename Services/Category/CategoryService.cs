using ModelCategory = Model.Models.Category;
using ModelTag = Model.Models.Tag;
using Repository;

namespace Services.Category;

public class CategoryService
{
    private readonly RepositoryManager _repositoryManager;

    public CategoryService(RepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ModelCategory> CreateCategoryAsync(string name, CancellationToken ct)
    {
        var category = new ModelCategory { Name = name };
        _repositoryManager.Category.CreateCategory(category);
        await _repositoryManager.SaveAsync();
        return category;
    }

    public async Task<bool> DeleteCategoryAsync(string name, CancellationToken ct)
    {
        var category = await _repositoryManager.Category.GetCategoryAsync(name, true);
        if (category == null) return false;
        _repositoryManager.Category.DeleteCategory(category);
        await _repositoryManager.SaveAsync();
        return true;
    }

    public async Task<ModelTag> CreateTagAsync(string categoryName, string tagName, CancellationToken ct)
    {
        var category = await _repositoryManager.Category.GetCategoryAsync(categoryName, true);
        if (category == null)
        {
            category = new ModelCategory { Name = categoryName };
            _repositoryManager.Category.CreateCategory(category);
        }

        var tag = new ModelTag { Name = tagName, Category = categoryName, CategoryNavigation = category };
        _repositoryManager.Tag.CreateTag(tag);
        await _repositoryManager.SaveAsync();
        return tag;
    }

    public async Task<bool> DeleteTagAsync(string categoryName, string tagName, CancellationToken ct)
    {
        var tag = await _repositoryManager.Tag.GetTagAsync(tagName, categoryName, true);
        if (tag == null) return false;
        _repositoryManager.Tag.DeleteTag(tag);
        await _repositoryManager.SaveAsync();
        return true;
    }
}

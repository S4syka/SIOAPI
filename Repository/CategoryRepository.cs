using Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Repository;

public class CategoryRepository(OaDbContext repositoryContext) : RepositoryBase<Category>(repositoryContext)
{
    public async Task<Category?> GetCategoryAsync(string categoryName, bool trackChanges) =>
        await FindByCondition(c => c.Name == categoryName, trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Category>> GetAllCategoriesAsync(bool trackChanges) =>
        await FindAll(trackChanges)
            .OrderBy(c => c.Name)
            .ToListAsync();

    public void DeleteCategory(Category category) => Delete(category);

    public void CreateCategory(Category category) => Create(category);
}

using Microsoft.EntityFrameworkCore;
using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class TagRepository(OaDbContext repositoryContext) : RepositoryBase<Tag>(repositoryContext)
{
    public async Task<Tag?> GetTagAsync(string tagName, string categoryName, bool trackChanges) =>
        await FindByCondition(t => t.Name == tagName && t.Category == categoryName, trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Tag>> GetAllTagsAsync(bool trackChanges) =>
        await FindAll(trackChanges)
            .OrderBy(t => t.Name)
            .ToListAsync();

    public void DeleteTag(Tag tag) => Delete(tag);

    public void CreateTag(Tag tag) => Create(tag);
}

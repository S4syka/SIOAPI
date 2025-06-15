using Model.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository;

public class TestRepository(OaDbContext repositoryContext) : RepositoryBase<Test>(repositoryContext)
{
    public async Task<Test?> GetTestAsync(Guid testId, bool trackChanges) =>
        await FindByCondition(t => t.Id.Equals(testId), trackChanges)
            .SingleOrDefaultAsync();

    public async Task<IEnumerable<Test>> GetAllTestsAsync(bool trackChanges) =>
        await FindAll(trackChanges)
            .OrderBy(t => t.Name)
            .ToListAsync();

    public void DeleteTest(Test test) => Delete(test);

    public void CreateTest(Test test) => Create(test);
}

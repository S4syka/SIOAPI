using Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repository.S3;
using Amazon.S3;
using Amazon.Runtime.Internal.Util;
using Microsoft.Extensions.Configuration;

namespace Repository;

public class RepositoryManager(OaDbContext repositoryContext)
{
    private readonly OaDbContext _repositoryContext = repositoryContext;

    private readonly Lazy<TestRepository> _testRepository = new Lazy<TestRepository>(() => new TestRepository(repositoryContext));

    private readonly Lazy<TagRepository> _tagRepository = new Lazy<TagRepository>(() => new TagRepository(repositoryContext));

    private readonly Lazy<CategoryRepository> _categoryRepository = new Lazy<CategoryRepository>(() => new CategoryRepository(repositoryContext));

    public TestRepository Test => _testRepository.Value;

    public TagRepository Tag => _tagRepository.Value;

    public CategoryRepository Category => _categoryRepository.Value;

    public async Task SaveAsync() => await _repositoryContext.SaveChangesAsync();
}
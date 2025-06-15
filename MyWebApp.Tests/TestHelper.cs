using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.Models;
using Repository;

namespace MyWebApp.Tests;

public static class TestHelper
{
    public static RepositoryManager CreateRepositoryManager(out TestDbContext context)
    {
        var options = new DbContextOptionsBuilder<OaDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        context = new TestDbContext(options);
        var configuration = new ConfigurationBuilder().Build();
        return new RepositoryManager(context, configuration);
    }

}

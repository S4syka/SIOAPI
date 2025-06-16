using Amazon.Runtime;
using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Model.Models;
using Repository;
using Repository.S3;
using Contracts;

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
        return new RepositoryManager(context);
    }

    public static ITestImageRepository CreateImageRepository(bool useMock = true)
    {
        return new TestImageMockRepository();

        //if (useMock)
        //{
            //return new TestImageMockRepository();
        //}

        //var configuration = new ConfigurationBuilder().Build();
        //var s3Client = new AmazonS3Client(new AnonymousAWSCredentials(), new AmazonS3Config());
        //ILogger logger = Logger.GetLogger(typeof(TestImageRepository));
        //return new TestImageRepository(s3Client, configuration, logger);
    }
}

using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Contracts;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.S3;

public class TestImageRepository : S3FileRepositoryBase, ITestImageRepository
{
    // TODO: inject S3FileRepositoryBase as a dependency
    private readonly ILogger _logger;

    public TestImageRepository(IAmazonS3 s3, IConfiguration config, ILogger logger) : base(s3, config, logger)
    {
        _logger = logger;
        BucketName = config["S3:BucketName"] ?? throw new ArgumentNullException("BucketName");
        // e.g. configure in appsettings.json: { "AWS": { "BucketName": "my-bucket" } }
    }

    public async Task<(Stream? data, string contentType)> DownloadTestImageAsync(Guid testId, string testName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(testName, nameof(testName));
        if (testId == Guid.Empty) throw new ArgumentNullException(nameof(testId));

        var key = $"test-images/{testId}/{testName}";

        try
        {
            return await DownloadAsync(key);
        }
        catch (Exception e)
        {
            _logger.Error(e, "S3 download failed for {Key}", key);
            return (null, string.Empty);
        }
    }

    public async Task UploadImageAsync(Guid testId, string testName, Stream data, string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(testName);
        if(testId == Guid.Empty) throw new ArgumentNullException(nameof(testId));

        var key = $"test-images/{testId}/{testName}";

        try
        {
            await UploadAsync(key, data, contentType);
        }
        catch(Exception e)
        {
            _logger.Error(e, "Failed to upload image to s3", testId, testName);
            throw;
        }
    }
}

using Contracts;

namespace Repository.S3;


public class TestImageMockRepository() : S3FileRepositoryMockBase, ITestImageRepository
{
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
            //_logger.Error(e, "S3 download failed for {Key}", key);
            return (null, string.Empty);
        }
    }

    public async Task UploadImageAsync(Guid testId, string testName, Stream data, string contentType)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(testName);
        if (testId == Guid.Empty) throw new ArgumentNullException(nameof(testId));

        var key = $"test-images/{testId}/{testName}";

        try
        {
            await UploadAsync(key, data, contentType);
        }
        catch (Exception e)
        {
            //_logger.Error(e, "Failed to upload image to s3", testId, testName);
            throw;
        }
    }
}

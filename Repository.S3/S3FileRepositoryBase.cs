using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.Net;

namespace Repository.S3;

public class S3FileRepositoryBase : IFileRepository
{
    private readonly ILogger _logger;
    private readonly IAmazonS3 _s3;

    public string? BucketName { get; set; }

    public S3FileRepositoryBase(IAmazonS3 s3, IConfiguration config, ILogger logger)
    {
        _logger = logger;
        _s3 = s3;
    }

    public async Task UploadAsync(string key, Stream data, string contentType)
    {
        if (data == null || data.Length == 0)
        {
            throw new ArgumentException("Stream is empty", nameof(data));
        }

        try
        {
            var response = await _s3.PutObjectAsync(new PutObjectRequest
            {
                BucketName = BucketName,
                Key = key,
                InputStream = data,
                ContentType = contentType
            });

            if (response.HttpStatusCode != HttpStatusCode.OK)
            {
                throw new Exception($"S3 returned non-OK status: {response.HttpStatusCode}");
            }
        }
        catch (AmazonS3Exception s3Ex)
        {
            _logger.Error(s3Ex, "S3 upload failed for {Key}", key);
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Unexpected error uploading {Key}", key);
            throw;
        }
    }

    public async Task<(Stream? data, string contentType)> DownloadAsync(string key)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(key, nameof(key));

        try
        {
            var response = await _s3.GetObjectAsync(BucketName, key);
            string contentType = response.Headers.ContentType;
            return (response.ResponseStream, contentType);
        }
        catch (AmazonS3Exception e) 
        {
            if(e.StatusCode == HttpStatusCode.NotFound)
            {
                return (null, string.Empty);
            }
            else
            {
                _logger.Error(e, "S3 download failed for {Key}", key);
                throw;
            }
        }
    }

    public async Task DeleteAsync(string key)
    {
        try
        {
            await _s3.DeleteObjectAsync(BucketName, key);
        }
        catch(AmazonS3Exception e)
        {
            if (e.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }
            else
            {
                _logger.Error(e, "S3 delete failed for {Key}", key);
                throw;
            }
        }
    }

    public async Task<IEnumerable<string>> ListKeysAsync(string? prefix = null)
    {
        var keys = new List<string>();
        string? continuationToken = null;

        do
        {
            var req = new ListObjectsV2Request
            {
                BucketName = BucketName,
                Prefix = prefix,
                ContinuationToken = continuationToken
            };
            var resp = await _s3.ListObjectsV2Async(req);
            keys.AddRange(resp.S3Objects.Select(o => o.Key!));
            continuationToken = resp.NextContinuationToken;
        } while (continuationToken != null);

        return keys;
    }
}

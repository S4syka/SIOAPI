using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.S3;

// 1. Define your repository interface
public interface IFileRepository
{
    /// <summary>
    /// Uploads a stream to S3 under the given key.
    /// </summary>
    Task UploadAsync(string key, Stream data, string contentType);

    /// <summary>
    /// Downloads the object at the given key, or null if not found.
    /// </summary>
    Task<(Stream? data, string contentType)> DownloadAsync(string key);

    /// <summary>
    /// Deletes the object at the given key.
    /// </summary>
    Task DeleteAsync(string key);

    /// <summary>
    /// Lists all keys under the given prefix.
    /// </summary>
    Task<IEnumerable<string>> ListKeysAsync(string? prefix = null);
}

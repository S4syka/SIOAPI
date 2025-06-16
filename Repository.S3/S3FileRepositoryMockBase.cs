using System.Net.Mime;

namespace Repository.S3;

public class S3FileRepositoryMockBase : IFileRepository
{
    static Dictionary<string, (byte[] data, string contentType)> _files = new Dictionary<string, (byte[] data, string contentType)>();

    public Task DeleteAsync(string key)
    {
        if (_files.ContainsKey(key))
        {
            _files.Remove(key);
        }
        return Task.CompletedTask;
    }

    public Task<(Stream data, string contentType)> DownloadAsync(string key)
    {
        string contentType = string.Empty;

        if (_files.ContainsKey(key))
        {
            contentType = _files[key].contentType;
            return (Task.FromResult(((Stream) new MemoryStream(_files[key].data), contentType)));
        }
        return Task.FromResult<(Stream data, string contentType)>((null, contentType));
    }

    public Task<IEnumerable<string>> ListKeysAsync(string? prefix = null)
    {
        if (string.IsNullOrEmpty(prefix))
        {
            return Task.FromResult(_files.Keys.AsEnumerable());
        }
        else
        {
            return Task.FromResult(_files.Keys.Where(k => k.StartsWith(prefix)));
        }
    }

    public async Task UploadAsync(string key, Stream data, string contentType)
    {
        if (_files.ContainsKey(key))
        {
            _files[key] = (await ReadAllBytesAsync(data), contentType);
        }
        else
        {
            _files.Add(key, (await ReadAllBytesAsync(data), contentType));
        }
    }

    public async Task<byte[]> ReadAllBytesAsync(Stream input)
    {
        if (input is MemoryStream ms && ms.TryGetBuffer(out _))
        {
            // Already in memory, just return its buffer
            return ms.ToArray();
        }

        using var buffer = new MemoryStream();
        await input.CopyToAsync(buffer);
        return buffer.ToArray();
    }
}

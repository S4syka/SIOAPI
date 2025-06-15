using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts;

public interface ITestImageRepository
{
    Task<(Stream? data, string contentType)> DownloadTestImageAsync(Guid testId, string testName);
    Task UploadImageAsync(Guid testId, string testName, Stream data, string contentType);
}

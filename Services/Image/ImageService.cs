using Contracts;

namespace Services.Image;

public class ImageService
{
    private readonly ITestImageRepository _imageRepository;

    public ImageService(ITestImageRepository imageRepository)
    {
        _imageRepository = imageRepository;
    }

    public Task<(Stream? data, string contentType)> DownloadAsync(Guid testId, string imageName)
        => _imageRepository.DownloadTestImageAsync(testId, imageName);

    public Task UploadAsync(Guid testId, string imageName, Stream data, string contentType)
        => _imageRepository.UploadImageAsync(testId, imageName, data, contentType);
}

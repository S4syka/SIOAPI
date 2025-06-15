using DataTransferObjects.Requests;
using FastEndpoints;
using Repository;

namespace MyWebApp.Endpoints;

public class GetImage : Endpoint<GetImageRequest>
{
    public required RepositoryManager RepositoryManager { get; set; }

    public override void Configure()
    {
        Get("/api/image/{testId}/{imageName}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(GetImageRequest req, CancellationToken ct)
    {
        var file = await RepositoryManager.TestImage.DownloadTestImageAsync(req.TestId, req.ImageName)!;

        if(file.data is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendStreamAsync(
                stream: file.data!,
                fileLengthBytes: file.data!.Length,
                contentType: file.contentType);
    }
}

using DataTransferObjects.Requests;
using FastEndpoints;
using Services.Image;

namespace MyWebApp.Endpoints;

public class GetImage : Endpoint<GetImageRequest>
{
    public required ImageService ImageService { get; set; }

    public override void Configure()
    {
        Get("/api/image/{testId}/{imageName}");
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Download a test image";
            s.Description = "Retrieves the specified image associated with a test.";
            s.Response(200, "Image stream");
            s.Response(404, "Image not found");
        });
    }

    public override async Task HandleAsync(GetImageRequest req, CancellationToken ct)
    {
        var file = await ImageService.DownloadAsync(req.TestId, req.ImageName);

        if (file.data is null)
        {
            await SendNotFoundAsync();
            return;
        }

        await SendStreamAsync(
            stream: file.data,
            fileLengthBytes: file.data.Length,
            contentType: file.contentType);
    }
}

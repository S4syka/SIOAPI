using Contracts;
using DataTransferObjects.Requests;
using FastEndpoints;
using Repository;

namespace MyWebApp.Endpoints;

public class PostImage : Endpoint<PostImageRequest>
{
    public required RepositoryManager RepositoryManager { get; set; }
    public required ITestImageRepository TestImageRepo { get; set; }


    public override void Configure()
    {
        Post("/api/image/{testId}/{imageName}");
        AllowFileUploads();
        AllowAnonymous();
        Summary(s =>
        {
            s.Summary = "Upload a test image";
            s.Description = "Uploads an image file for the specified test.";
            s.Response(200, "Image uploaded successfully");
        });
    }

    public override async Task HandleAsync(PostImageRequest req, CancellationToken ct)
    {
        await TestImageRepo.UploadImageAsync(req.TestId, req.ImageName, req.Image.OpenReadStream(), req.Image.ContentType);
        await SendOkAsync();
        return;
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Requests;

public class PostImageRequest
{
    public required Guid TestId { get; set; }

    public required string ImageName{ get; set; }

    public required IFormFile Image { get; set; }
}

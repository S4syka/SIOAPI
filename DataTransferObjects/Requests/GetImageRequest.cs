using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Requests;

public class GetImageRequest
{
    public required Guid TestId { get; set; }
    public required string ImageName { get; set; }
}

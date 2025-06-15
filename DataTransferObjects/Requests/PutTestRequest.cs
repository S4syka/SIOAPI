using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Requests;

public class PutTestRequest
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public required string Content { get; set; }
    public required List<TagDTO> Tags { get; set; }

    public class TagDTO
    {
        public required string TagName { get; set; }
        public required string CategoryName { get; set; }
    }
}

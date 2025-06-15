using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Responses;

public class GetTestsResponse
{
    public required List<TestElement> Tests { get; set; }

    public class TestElement
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required Dictionary<string, List<string>> Tags { get; set; }
    }
}

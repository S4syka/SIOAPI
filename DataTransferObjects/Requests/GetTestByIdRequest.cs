using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransferObjects.Requests;

public class GetTestByIdRequest
{
    public Guid Id { get; set; }
}

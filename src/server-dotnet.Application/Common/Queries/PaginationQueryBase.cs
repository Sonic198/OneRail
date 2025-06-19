using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_dotnet.Application.Common.Queries;
public record PaginationQueryBase
{    
    public int PageSize { get; init; }
    public int PageNumber { get; init; }
}

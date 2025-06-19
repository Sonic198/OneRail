using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace server_dotnet.Application.Modules.Organizations.Dtos;
public record OrganizationDto
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Industry { get; init; }
    public DateTime DateFounded { get; init; }
}

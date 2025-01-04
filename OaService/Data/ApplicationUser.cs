using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace OaService.Data;

public class ApplicationUser : IdentityUser<Guid>
{
    public bool IsActive { get; set; } = true;

    [Column(TypeName = "nvarchar(50)")]
    public string? ManagerId {get;set;}
} 
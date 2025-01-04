using OaService.Data;

namespace OaService.Models;

public class CreateLeaveRequest
{
    public required ApplicationType ApplicationType { get; set; }
    public required DateTime StartDate { get; set; }
    public required DateTime EndDate { get; set; }
    public required string ApplicationReason { get; set; }
} 
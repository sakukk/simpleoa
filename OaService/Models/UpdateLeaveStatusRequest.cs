namespace OaService.Models;

public class UpdateLeaveStatusRequest
{
    public required ApplicationStatus Status { get; set; }
    public string? ApproverReason { get; set; }
} 
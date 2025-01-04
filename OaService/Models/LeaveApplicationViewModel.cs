using OaService.Data;

namespace OaService.Models;

public record LeaveApplicationViewModel(
    Guid Id,
    Guid ApplicantId,
    string ApplicantEmail,
    ApplicationType ApplicationType,
    ApplicationStatus Status,
    DateTime StartDate,
    DateTime EndDate,
    string ApplicationReason,
    string? ApproverReason,
    DateTime CreatedAt,
    DateTime? ApprovedAt,
    string? Comment
); 
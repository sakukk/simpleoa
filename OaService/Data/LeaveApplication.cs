using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OaService.Data;

public class LeaveApplication
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ApplicantId { get; set; }

    public ApplicationType ApplicationType {get;set;}

    public ApplicationStatus Status { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [ForeignKey("ApplicantId")]
    public ApplicationUser Applicant { get; set; } = null!;

    [Required]
    [MaxLength(500)]
    public string ApplicationReason { get; set; } = string.Empty;

    public Guid? ApprovedById { get; set; }

    [ForeignKey("ApprovedById")]
    public ApplicationUser Approver { get; set; } = null!;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? ApprovedAt { get; set; }

    [MaxLength(500)]
    public string ApproverReason { get; set; } = string.Empty;
}

public enum ApplicationType 
{
    AnnualLeave = 0,
    PersonalLeave = 1
}

public enum ApplicationStatus 
{
    Pending = 0,
    Approved = 1,
    Rejected = 2
}
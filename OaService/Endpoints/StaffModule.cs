using System.Security.Claims;
using Carter;
using Microsoft.EntityFrameworkCore;
using OaService.Data;
using OaService.Models;

namespace OaService.Endpoints;

public class StaffModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/staff/leave-applications", async (
            HttpContext httpContext,
            ApplicationDbContext dbContext) =>
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var applicantId))
            {
                return Results.Unauthorized();
            }

            var applications = await dbContext.LeaveApplications
                .Include(x => x.Applicant)
                .Where(x => x.ApplicantId == applicantId)
                .Select(x => new LeaveApplicationViewModel(
                    x.Id,
                    x.ApplicantId,
                    x.Applicant.Email!,
                    x.ApplicationType,
                    x.Status,
                    x.StartDate,
                    x.EndDate,
                    x.ApplicationReason,
                    x.ApproverReason,
                    x.CreatedAt,
                    x.ApprovedAt,
                    x.ApproverReason
                ))
                .ToListAsync();

            return Results.Ok(applications);
        }).RequireAuthorization("RequireStaff");

        app.MapPost("api/staff/leave-applications", async (
            HttpContext httpContext,
            ApplicationDbContext dbContext,
            CreateLeaveRequest request) =>
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var applicantId))
            {
                return Results.Unauthorized();
            }

            var staff = await dbContext.Users.FirstOrDefaultAsync(u => u.Id == applicantId);

            if (staff == null)
            {
                return Results.Unauthorized();
            }

            if (!Guid.TryParse(staff?.ManagerId, out var managerId))
            {
                return Results.Unauthorized();
            }


            var leaveApplication = new LeaveApplication
            {
                ApplicantId = applicantId,
                ApplicationType = request.ApplicationType,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                ApplicationReason = request.ApplicationReason,
                Status = ApplicationStatus.Pending,
                ApprovedById = managerId
            };

            dbContext.LeaveApplications.Add(leaveApplication);
            await dbContext.SaveChangesAsync();

            return Results.Ok(leaveApplication);
        }).RequireAuthorization("RequireStaff");
    }
}
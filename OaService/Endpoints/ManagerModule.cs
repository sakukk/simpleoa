using System.Security.Claims;
using Carter;
using Microsoft.EntityFrameworkCore;
using OaService.Data;
using OaService.Models;

namespace OaService.Endpoints;

public class ManagerModule : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("api/manager/leave-applications", async (
            HttpContext httpContext,
            ApplicationDbContext dbContext) =>
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var managerId))
            {
                return Results.Unauthorized();
            }

            var applications = await dbContext.LeaveApplications
                .Include(x => x.Applicant)
                .Where(x => x.ApprovedById == managerId)
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
        }).RequireAuthorization("RequireManager");

        app.MapPut("api/manager/leave-applications/{id}", async (
            HttpContext httpContext,
            ApplicationDbContext dbContext,
            Guid id,
            UpdateLeaveStatusRequest request) =>
        {
            var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var managerId))
            {
                return Results.Unauthorized();
            }

            var application = await dbContext.LeaveApplications
                .FirstOrDefaultAsync(x => x.Id == id && x.ApprovedById == managerId);

            if (application == null)
            {
                return Results.NotFound();
            }

            if (request.Status == ApplicationStatus.Rejected && string.IsNullOrEmpty(request.ApproverReason))
            {
                return Results.BadRequest("Reason is required when rejecting application");
            }

            application.Status = request.Status;
            application.ApproverReason = request.ApproverReason ?? string.Empty;
            application.ApprovedAt = DateTime.UtcNow;

            await dbContext.SaveChangesAsync();
            return Results.Ok(application);
        }).RequireAuthorization("RequireManager");
    }
}
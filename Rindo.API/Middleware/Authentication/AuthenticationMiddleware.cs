using Application.Interfaces.Access;
using Application.Interfaces.Repositories;

namespace Rindo.API.Middleware.Authentication;

public class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context, IDataAccessController dataAccessController, IProjectRepository projectRepository)
    {
        if (context.User.Identity?.IsAuthenticated == true)
        {
            var userIdClaim = context.User.Claims.First(c => c.Type == "userId");
            dataAccessController.EmployeeId = Guid.Parse(userIdClaim.Value);
            dataAccessController.AccessibleProjectsIds = (await projectRepository.GetProjectsWhereUserAttends(dataAccessController.EmployeeId)).Select(x => x.ProjectId).ToArray();
        }
        await next(context);
    }
}
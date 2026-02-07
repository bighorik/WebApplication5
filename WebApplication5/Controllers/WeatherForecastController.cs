using System.Data;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController(IConfiguration config) : ControllerBase
    {
        private static List<string> Summaries = [
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        ];

        [Authorize]
        [HttpGet]
        public List<string> Get()
        {
            return Summaries;
        }

        [Authorize("Admin")]
        [Authorize("Moderator")]
        [HttpDelete]
        public List<string> Delete()
        {
            return Summaries;
        }

        [Authorize(["Admin", "Moderator"])]
        [HttpPost("{id}")]
        public void AddSummary([FromBody] SummaryDto dto, [FromRoute] Guid id)
        {
            Summaries.Add(dto.Name);
        }
    }
}


public class SummaryDto
{
    public string Name { get; set; }
}

public static class ClaimsExtension
{
    public static List<string> GetClientRoles(this ClaimsPrincipal user, string clientName)
    {
        var json = user.FindFirst("resource_access")?.Value;
        if (JsonDocument.Parse(json).RootElement.TryGetProperty(clientName, out var clientProp) &&
            clientProp.TryGetProperty("roles", out var rolesProp))
        {
            return rolesProp.EnumerateArray()
                           .Select(x => x.GetString() ?? "")
                           .Where(x => x != "")
                           .ToList();
        }
        else return [];
    }
}


[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class AuthorizeAttribute(string[]? roles = null) : Attribute, IAsyncAuthorizationFilter
{
    public Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        var a = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();

        if (!context.HttpContext.User.Identity?.IsAuthenticated ?? false)
        {
            throw new Exception("401 не авторизован");
        }
        var tokenRoles = context.HttpContext.User.GetClientRoles("account-console");
        bool authed = false;
        if (roles != null)
        {
            foreach (var tokenRole in tokenRoles)
            {
                if (roles.Contains(tokenRole))
                    authed = true;
            }
            if (!authed)
                throw new Exception("403 не имеет прав");
        }

        return Task.CompletedTask;

        // user, moderator, admin

        // get - user, moderator, admin
        // post - user, moderator, admin
        // delete - moderator, admin
        // put - admin
    }
}
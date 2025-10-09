using AmpApp.Shared.Models.Todo;
using Microsoft.AspNetCore.Mvc;
using Zamp.Server.Infrastructure.Middleware;

namespace AmpApp.Features.Todo;

public class Endpoints : CarterModule
{
    public override void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/todo", async (CreateService service, TodoEntity row, HttpContext http) =>
            {
                var result = await service.HandleAsync(row);
                var location = $"/api/todo/{result.Id}";
                http.Response.Headers.Location = location;
                return Results.Created(location, result);
            })
            .RequireAuthorization(AppPolicies.EditorOrHigher)
            .AddEndpointFilter<ValidationFilter<TodoEntity>>();

        app.MapPut("/api/todo/{id:guid}", async (Guid id, TodoDto dto, UpdateService service) =>
            {
                var result = await service.HandleAsync(id, dto);
                return result is null ? Results.NotFound() : Results.Ok(result);
            })
            .RequireAuthorization(AppPolicies.EditorOrHigher);

        app.MapDelete("/api/todo/{id:guid}", async (Guid id, DeleteService service) =>
            {
                var deleted = await service.HandleAsync(id);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .RequireAuthorization(AppPolicies.EditorOrHigher);

        app.MapGet("/api/todo/{id:guid}", async (Guid id, GetByIdService service) =>
            {
                var todo = await service.HandleAsync(id);
                return todo is null ? Results.NotFound() : Results.Ok(todo);
            })
            .RequireAuthorization(AppPolicies.GuestOrHigher);

        app.MapGet("/api/todo/all", async (GetAllService service) =>
            {
                var todos = await service.HandleAsync();
                return Results.Ok(todos);
            })
            .RequireAuthorization(AppPolicies.GuestOrHigher);

        // NOTE: POST is used for all search endpoints. Parsing querystring on API side easier, 
        // On API side, parsing the Request Body for criteria is easier than parsing the querystring (especially with nested objects)
        // On Client side, creating the Request Body is easier creating a querystring (especially with nested objects)
        app.MapPost("/api/todo/search", async (
            [FromServices] GetByCriteriaService service,
            [FromBody] TodoGridCriteriaModel criteria) =>
            {
                try
                {
                    var response = await service.GetAsync(criteria);
                    return Results.Ok(response);
                }
                catch (DataAccessException ex)
                {
                    // Repository threw a custom exception (you can log or customize the response)
                    return Results.Problem(
                        detail: ex.Message,
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "A database error occurred while retrieving the data.");
                }
                catch (Exception)
                {
                    // General fallback
                    return Results.Problem(
                        detail: "An unexpected error occurred.",
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Server Error");
                }
            }).RequireAuthorization(AppPolicies.GuestOrHigher);
    }
}
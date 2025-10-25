using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/todos");

        group.MapGet("/", [Authorize] async ([FromServices] ITodoService svc) =>
        {
            var todos = await svc.GetAllAsync();

            return TypedResults.Ok(todos);
        });

        group.MapGet("/{id}", [Authorize] async (int id, [FromServices] ITodoService svc) =>
        {
            var found = await svc.GetByIdAsync(id);
            return found is null
                ? Results.NotFound()
                : Results.Ok(found);
        });

        group.MapPost("/", [Authorize] async ([FromBody] TodoDto todoDto, IValidator<TodoDto> validator, [FromServices] ITodoService svc) =>
            {
                var result = await validator.ValidateAsync(todoDto);
                if (!result.IsValid)
                    return Results.ValidationProblem(result.ToDictionary());

                var todo = new Todo
                {
                    Title = todoDto.Title!,
                    IsComplete = (bool)todoDto.IsComplete!
                };

                await svc.AddAsync(todo);

                return TypedResults.Created($"/todos/{todo.Id}", todo);
            });

        group.MapPatch("/{id}", [Authorize] async (int id, TodoDto inputTodo, IValidator<TodoDto> validator, [FromServices] ITodoService svc) =>
        {
            var result = await validator.ValidateAsync(inputTodo);
            if (!result.IsValid)
                return Results.ValidationProblem(result.ToDictionary());

            var found = await svc.UpdateAsync(id, inputTodo.Title!, (bool)inputTodo.IsComplete!);
            if (!found)
            {
                return Results.NotFound();
            }

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id}", [Authorize] async (int id, [FromServices] ITodoService svc) =>
        {
            if (await svc.DeleteAsync(id))
            {
                return Results.NoContent();
            }

            return TypedResults.NotFound();
        });

        return endpoints;
    }
}

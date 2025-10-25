using Microsoft.AspNetCore.Mvc;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/todos");

        group.MapGet("/", async ([FromServices] ITodoService svc) =>
        {
            var todos = await svc.GetAllAsync();

            return TypedResults.Ok(todos);
        });

        group.MapGet("/{id}", async (int id, [FromServices] ITodoService svc) =>
        {
            var found = await svc.GetByIdAsync(id);
            return found is null
                ? Results.NotFound()
                : Results.Ok(found);
        });

        group.MapPost("/", async ([FromBody] TodoDto todoDto, [FromServices] ITodoService svc) =>
            {
                var todo = new Todo
                {
                    Title = todoDto.Title,
                    IsComplete = todoDto.IsComplete
                };

                await svc.AddAsync(todo);

                return TypedResults.Created($"/todos/{todo.Id}", todo);
            });

        group.MapPut("/{id}", async (int id, Todo inputTodo, [FromServices] ITodoService svc) =>
        {
            var todo = await svc.GetByIdAsync(id);
            if (todo is null) return Results.NotFound();

            todo.Title = inputTodo.Title;
            todo.IsComplete = inputTodo.IsComplete;
            await svc.UpdateAsync(todo);

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id}", async (int id, [FromServices] ITodoService svc) =>
        {
            if (await svc.GetByIdAsync(id) is Todo todo)
            {
                await svc.DeleteAsync(todo);
                return Results.NoContent();
            }

            return TypedResults.NotFound();
        });

        return endpoints;
    }
}

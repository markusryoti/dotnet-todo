using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

public static class TodoEndpoints
{
    public static IEndpointRouteBuilder MapTodoEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/todos");

        group.MapGet("/", async (AppDbContext db) =>
        {
            var todos = await db.Todos.ToListAsync();

            return TypedResults.Ok(todos);
        });

        group.MapGet("/{id}", async (int id, AppDbContext db) =>
        {
            var found = await db.Todos.FindAsync(id);
            return found is null
                ? Results.NotFound()
                : Results.Ok(found);
        });

        group.MapPost("/", async ([FromBody] TodoDto todoDto, AppDbContext db) =>
            {
                var todo = new Todo
                {
                    Title = todoDto.Title,
                    IsComplete = todoDto.IsComplete
                };

                db.Todos.Add(todo);
                await db.SaveChangesAsync();

                return TypedResults.Created($"/todos/{todo.Id}", todo);
            });

        group.MapPut("/{id}", async (int id, Todo inputTodo, AppDbContext db) =>
        {
            var todo = await db.Todos.FindAsync(id);
            if (todo is null) return Results.NotFound();

            todo.Title = inputTodo.Title;
            todo.IsComplete = inputTodo.IsComplete;
            await db.SaveChangesAsync();

            return TypedResults.NoContent();
        });

        group.MapDelete("/{id}", async (int id, AppDbContext db) =>
        {
            if (await db.Todos.FindAsync(id) is Todo todo)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
                return Results.NoContent();
            }

            return TypedResults.NotFound();
        });

        return endpoints;
    }
}

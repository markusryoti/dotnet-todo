using Microsoft.EntityFrameworkCore;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(int id);
    Task AddAsync(Todo todo);
    Task<bool> UpdateAsync(int id, Todo todo);
    Task<bool> DeleteAsync(int id);
}

public class TodoService : ITodoService
{
    private readonly AppDbContext _db;

    public TodoService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<IEnumerable<Todo>> GetAllAsync()
        => await _db.Todos.ToListAsync();

    public async Task<Todo?> GetByIdAsync(int id)
        => await _db.Todos.FindAsync(id);

    public async Task AddAsync(Todo todo)
    {
        _db.Todos.Add(todo);
        await _db.SaveChangesAsync();
    }

    public async Task<bool> UpdateAsync(int id, Todo input)
    {
        var todo = await _db.Todos.FindAsync(id);
        if (todo is null) return false;

        todo.Title = input.Title;
        todo.IsComplete = input.IsComplete;

        await _db.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existingTodo = await _db.Todos.FindAsync(id);
        if (existingTodo is null) return false;

        _db.Todos.Remove(existingTodo);
        await _db.SaveChangesAsync();

        return true;
    }
}

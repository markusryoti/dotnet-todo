using Microsoft.EntityFrameworkCore;

public interface ITodoService
{
    Task<IEnumerable<Todo>> GetAllAsync();
    Task<Todo?> GetByIdAsync(int id);
    Task AddAsync(Todo todo);
    Task UpdateAsync(Todo todo);
    Task DeleteAsync(Todo todo);
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

    public async Task UpdateAsync(Todo todo)
    {
        _db.Todos.Update(todo);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Todo todo)
    {
        _db.Todos.Remove(todo);
        await _db.SaveChangesAsync();
    }
}

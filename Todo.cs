public class Todo
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public bool IsComplete { get; set; } = false;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

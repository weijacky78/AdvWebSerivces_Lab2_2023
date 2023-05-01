
namespace TodoApi.Models;

public class TodoItem
{

    public uint TodoItemId { get; set; }

    public string? Task { get; set; }

    public bool IsComplete { get; set; }

}
using Microsoft.EntityFrameworkCore;
using ToDoAPI;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TodoDb>(options =>
    options.UseInMemoryDatabase("TodoList"));
var app = builder.Build();



app.MapPost("/todoitems", async (TodoItem todoItem, TodoDb db) =>
{
    db.TodoItems.Add(todoItem);
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
});

app.MapPut("/todoitems/{id}", async (int id,TodoItem todoItem, TodoDb db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo == null)
    {
        return Results.NotFound();
    }
    todo.Name = todoItem.Name;
    todo.IsComplete = todoItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.Created($"/todoitems/{todoItem.Id}", todoItem);
});

app.MapDelete("/todoitems/{id}", async (int id, TodoDb db) =>
{
    var todo = await db.TodoItems.FindAsync(id);
    if (todo == null)
    {
        return Results.NotFound();
    }
    db.TodoItems.Remove(todo);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapGet("/todoitems", async (TodoDb db) =>
{
   return await db.TodoItems.ToListAsync();
});

app.MapGet("/todoitems/{id}", async (int id,TodoDb db) =>
{
    return await db.TodoItems.FindAsync(id);
});

app.Run();

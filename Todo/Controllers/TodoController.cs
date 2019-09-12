using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Todo.Controllers
{
    [Route("api/Todo")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        readonly Context ctx;
        public TodoController(Context context)
        {
            ctx = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetItems()
        {
            return await ctx.TodoItems.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(long id)
        {
            var item = await ctx.TodoItems.FindAsync(id);
            if (item == null)
                return NotFound();
            return item;
        }
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem item)
        {
            ctx.TodoItems.Add(item);
            await ctx.SaveChangesAsync();
            return CreatedAtAction(nameof(GetTodoItem), new { id = item.Id }, item);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(long id, TodoItem item)
        {
            if (id != item.Id)
            {
                return BadRequest();
            }
            ctx.Entry(item).State = EntityState.Modified;
            await ctx.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            var toDelete = await ctx.TodoItems.FindAsync(id);
            if (toDelete == null)
                return NotFound();
            ctx.TodoItems.Remove(toDelete);
            await ctx.SaveChangesAsync();

            return NoContent();
        }
    }
}

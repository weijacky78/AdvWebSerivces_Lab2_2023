using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [FormatFilter]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly TodoContext _context;

        public TodoItemsController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/TodoItems/x? (x is alpha and options)
        [HttpGet("{format:alpha?}")]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetTodoItems()
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            return await _context.TodoItems.ToListAsync();
        }

        // GET: api/TodoItem/x (x >=1)
        [HttpGet("{id:min(1)}/{format:alpha?}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(uint id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);

            if (todoItem == null)
            {
                return NotFound();
            }

            return todoItem;
        }
        // HTTP patch [PATCH]/api/TodoItems/5/true
        [HttpPatch("{id:int}")]
        public async Task<ActionResult<TodoItem>> PatchTodoItem(uint id, TodoItem updatedData)
        {
            if (id != updatedData.TodoItemId)
            {
                return BadRequest();
            }

            _context.Entry(updatedData).State = EntityState.Modified;

            await _context.SaveChangesAsync();
            return updatedData;

        }
        // HTTP patch [PATCH]/api/TodoItems/5/true
        [HttpPatch("{id:int}/{complete:bool}")]
        public async Task<ActionResult<TodoItem>> CompleteTodoItem(uint id, bool complete)
        {
            var todoObj = await _context.TodoItems.FindAsync(id); // find the record with the given id

            if (todoObj == null) // if does not exist, 404
            {
                return NotFound();
            }
            if (todoObj.IsComplete == complete) // if the record is already set to value of complete, don't do anything
            {
                return NoContent();
            }
            todoObj.IsComplete = complete; // set the value
            _context.Update(todoObj); // update context
            await _context.SaveChangesAsync(); // save to db

            return todoObj;

        }
        // PUT: api/TodoItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(uint id, TodoItem todoItem)
        {
            if (id != todoItem.TodoItemId)
            {
                return BadRequest();
            }

            _context.Entry(todoItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/TodoItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoItem)
        {
            if (_context.TodoItems == null)
            {
                return Problem("Entity set 'TodoContext.TodoItems'  is null.");
            }
            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodoItem", new { id = todoItem.TodoItemId }, todoItem);
        }

        // DELETE: api/TodoItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(uint id)
        {
            if (_context.TodoItems == null)
            {
                return NotFound();
            }
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }

            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoItemExists(uint id)
        {
            return (_context.TodoItems?.Any(e => e.TodoItemId == id)).GetValueOrDefault();
        }
    }
}

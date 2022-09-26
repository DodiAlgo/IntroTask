using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IntroTask.Models;

namespace IntroTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoTextsController : ControllerBase
    {
        private readonly ToDoContext _context;

        public ToDoTextsController(ToDoContext context)
        {
            _context = context;
        }

        // GET: api/ToDoTexts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ToDoText>>> GetToDoTexts()
        {
            return await _context.ToDoTexts.ToListAsync();
        }

        // GET: api/ToDoTexts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ToDoText>> GetToDoText(long id)
        {
            var toDoText = await _context.ToDoTexts.FindAsync(id);

            if (toDoText == null)
            {
                return NotFound();
            }

            return toDoText;
        }

        // PUT: api/ToDoTexts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutToDoText(long id, ToDoText toDoText)
        {
            if (id != toDoText.Id)
            {
                return BadRequest();
            }

            _context.Entry(toDoText).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ToDoTextExists(id))
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

        // POST: api/ToDoTexts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ToDoText>> PostToDoText(ToDoText toDoText)
        {
            _context.ToDoTexts.Add(toDoText);
            await _context.SaveChangesAsync();
            if (toDoText.Text != null)
            {
                string text = toDoText.Text;
                int l = 0;
                int wrd = 1;

                while (l <= text.Length - 1)
                {

                    if (text[l] == ' ' || text[l] == '\n' || text[l] == '\t')
                    {
                        wrd++;
                    }

                    l++;
                }
                toDoText.numWords = wrd;
                toDoText.IsComplete = true;
                await _context.SaveChangesAsync();
            }
            else
            {
                toDoText.numWords = 0;
                toDoText.IsComplete = false;
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction(nameof(GetToDoText), new { id = toDoText.Id }, toDoText);
        }

        // DELETE: api/ToDoTexts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoText(long id)
        {
            var toDoText = await _context.ToDoTexts.FindAsync(id);
            if (toDoText == null)
            {
                return NotFound();
            }

            _context.ToDoTexts.Remove(toDoText);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ToDoTextExists(long id)
        {
            return _context.ToDoTexts.Any(e => e.Id == id);
        }
    }
}

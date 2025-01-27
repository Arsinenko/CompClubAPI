using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {
        private readonly CollegeTaskContext _context;

        public FeedbacksController(CollegeTaskContext context)
        {
            _context = context;
        }

        // GET: api/Feedbacks
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks()
        {
            var results = from feedback in _context.Feedbacks
                join client in _context.Clients on feedback.IdClient equals client.Id
                select new
                {
                    id = feedback.Id,
                    rating = feedback.Rating,
                    comment = feedback.Comment,
                    login = client.Login,
        
                };
            return Ok(results);
        }

        // GET: api/Feedbacks/5
        [HttpGet("get_info/{id}")]
        public async Task<ActionResult<Feedback>> GetFeedback(int id)
        {
            var feedback = await _context.Feedbacks.FindAsync(id);

            if (feedback == null)
            {
                return NotFound();
            }

            return Ok( new { id = feedback.Id, rating = feedback.Rating, comment = feedback.Comment });
        }
        [Authorize]
        // PUT: api/Feedbacks/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("update/{id}")]
        public async Task<IActionResult> PutFeedback(int id, Feedback feedback)
        {
            if (id != feedback.Id)
            {
                return BadRequest();
            }

            _context.Entry(feedback).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeedbackExists(id))
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

        // POST: api/Feedbacks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("new_feedback")]
        public async Task<ActionResult<Feedback>> PostFeedback(CreateFeedbackModel feedbackModel)
        {
            int idClient = Convert.ToInt32(User.FindFirst("client_id")?.Value);
            Feedback feedback = new Feedback
            {
                IdClient = idClient,
                IdClub = feedbackModel.clubId,
                Rating = feedbackModel.rating,
                Comment = feedbackModel.comment
            };
            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFeedback", new { id = feedback.Id }, feedback);
        }

        private bool FeedbackExists(int id)
        {
            return _context.Feedbacks.Any(e => e.Id == id);
        }
    }
}

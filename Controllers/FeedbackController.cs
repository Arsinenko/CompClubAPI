using CompClubAPI.Models;
using CompClubAPI.Schemas;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CompClubAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        public readonly CollegeTaskContext _context;

        public FeedbackController(CollegeTaskContext context)
        {
            _context = context;
        }

        [Authorize(Roles = "Client")]
        [HttpPost("create_feedback")]
        public async Task<ActionResult> CreateFeedback(CreateFeedbackModel feedbackModel)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Feedback feedback = new Feedback
            {
                AccountId = accountId,
                IdClub = feedbackModel.ClubId,
                Rating = feedbackModel.Rating,
                Comment = feedbackModel.Comment,
                CreatedAt = DateTime.Now
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();
            
            return Created("", new { message = "Feedback created successfully!", id = feedback.Id });
        }

        [HttpGet]
        public async Task<ActionResult> GetFeedbacks()
        {
            List<Feedback> feedbacks = await _context.Feedbacks.ToListAsync();
            return Ok(feedbacks);
        }
        
        [Authorize(Roles = "Client")]
        [HttpGet("get_client_feedbacks")]
        public async Task<ActionResult> GetClientFeedbacks()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            List<Feedback> feedbacks = await _context.Feedbacks.Where(f => f.AccountId == accountId).ToListAsync();
            return Ok(feedbacks);
        }

        [Authorize(Roles = "Client")]
        [HttpPost("update_feedback/{id}")]
        public async Task<IActionResult> UpdateFeedback(int id, CreateFeedbackModel feedbackModel)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Feedback? feedback = await _context.Feedbacks.Where(f => f.AccountId == accountId && f.Id == id).FirstOrDefaultAsync();
            
            if (feedback == null)
            {
                return BadRequest(new { error = "Feedback not found or not yours!" });
            }
            
            feedback.Rating = feedbackModel.Rating;
            feedback.Comment = feedbackModel.Comment;
            feedback.CreatedAt = DateTime.Now;
            
            _context.Feedbacks.Update(feedback);
            await _context.SaveChangesAsync();
            
            return Ok(new {message = "Feedback updated successfully!"});
        }
        [Authorize(Roles = "Client")]
        [HttpDelete("delete_feedback/{id}")]
        public async Task<IActionResult> DeleteFeedback(int id)
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            Feedback? feedback = await _context.Feedbacks.Where(f => f.AccountId == accountId && f.Id == id).FirstOrDefaultAsync();
            
            if (feedback == null)
            {
                return BadRequest(new { error = "Feedback not found or not yours!" });
            }
            
            _context.Feedbacks.Remove(feedback);
            await _context.SaveChangesAsync();
            return Ok(new {message = "Feedback deleted successfully!"});
        }
        
    }
}

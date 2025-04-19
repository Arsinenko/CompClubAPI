using CompClubAPI.Context;
using CompClubAPI.Models;
using CompClubAPI.ResponseSchema;
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
        private readonly CollegeTaskV2Context _context;

        public FeedbackController(CollegeTaskV2Context context)
        {
            _context = context;
        }
        /// <summary>
        /// Создание отзыва. Для клиента.
        /// </summary>
        /// <param name="feedbackModel">Модель отзыва</param>
        /// <returns>Сообщение об успешном создании отзыва</returns>
        /// <remarks>Доступно только для роли Client</remarks>
        [Authorize(Roles = "Client")]
        [HttpPost("create_feedback")]
        [ProducesResponseType(typeof(MessageResponse), StatusCodes.Status201Created)]
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
        
        /// <summary>
        /// Получение всех отзывов.
        /// </summary>
        /// <returns>Список отзывов с информацией о клиенте и клубе</returns>
        [HttpGet("get_feedbacks")]
        public async Task<ActionResult> GetFeedbacks()
        {
            var result = await (from feedback in _context.Feedbacks
                join account in _context.Accounts on feedback.AccountId equals account.Id
                join client in _context.Clients on account.IdClient equals client.Id
                join club in _context.Clubs on feedback.IdClub equals club.Id
                select new
                {
                    Rating = feedback.Rating,
                    Comment = feedback.Comment,
                    Name = client.FirstName,
                    Address = club.Address,
                    CreatedAt = feedback.CreatedAt
                }).ToListAsync();
            return Ok(new {result});
        }
        
        /// <summary>
        /// Получение отзывов текущего клиента.
        /// </summary>
        /// <returns>Список отзывов клиента</returns>
        /// <remarks>Доступно только для роли Client</remarks>
        [Authorize(Roles = "Client")]
        [HttpGet("get_client_feedbacks")]
        public async Task<ActionResult> GetClientFeedbacks()
        {
            int accountId = Convert.ToInt32(User.FindFirst("account_id")?.Value);
            List<Feedback> feedbacks = await _context.Feedbacks.Where(f => f.AccountId == accountId).ToListAsync();
            return Ok(feedbacks);
        }

        /// <summary>
        /// Обновление отзыва клиента.
        /// </summary>
        /// <param name="id">Идентификатор отзыва</param>
        /// <param name="feedbackModel">Модель отзыва с новыми данными</param>
        /// <returns>Сообщение об успешном обновлении отзыва</returns>
        /// <remarks>Доступно только для роли Client</remarks>
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
        /// <summary>
        /// Удаление отзыва клиента.
        /// </summary>
        /// <param name="id">Идентификатор отзыва</param>
        /// <returns>Сообщение об успешном удалении отзыва</returns>
        /// <remarks>Доступно только для роли Client</remarks>
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

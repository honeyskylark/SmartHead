using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Vote
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Пользователь")]
        public int? UserId { get; set; }
        public User User { get; set; }

        [Display(Name = "Отзыв")]
        public int? FeedbackId { get; set; }
        public Feedback Feedback { get; set; }

        [Required(ErrorMessage = "Необходимо указать оценку")]
        [Display(Name = "Оценка")]
        public int RatingGiven { get; set; }

        [Required(ErrorMessage = "Необходимо указать время")]
        [Display(Name = "Время")]
        public DateTime Time { get; set; }

        [Display(Name = "Голос возвращен")]
        public bool VoteReturned { get; set; }
    }
}

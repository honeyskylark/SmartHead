using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Feedback
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Display(Name = "Пользователь")]
        public int? UserId { get; set; }
        public User User { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Необходимо заполнить название")]
        [Display(Name = "Название")]
        public string Name { get; set; }

        [StringLength(1000)]
        [Display(Name = "Контент")]
        public string Content { get; set; }
    }
}

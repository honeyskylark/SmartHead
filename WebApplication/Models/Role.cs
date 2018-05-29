using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class Role
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [StringLength(20)]
        [Required(ErrorMessage = "Необходимо заполнить роль")]
        [Display(Name = "Роль")]
        public string Name { get; set; }
    }
}

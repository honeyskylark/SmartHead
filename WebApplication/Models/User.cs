using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models
{
    public class User
    {
        [Display(Name = "ID")]
        public int Id { get; set; }

        [StringLength(16)]
        [Required(ErrorMessage = "Необходимо заполнить имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        [StringLength(16)]
        [Display(Name = "Отчество")]
        public string Patronymic { get; set; }

        [StringLength(16)]
        [Display(Name = "Фамилия")]
        public string LastName { get; set; }

        [StringLength(16)]
        [Required(ErrorMessage = "Необходимо заполнить логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить пароль")]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Необходимо указать роль")]
        [Display(Name = "Роль")]
        public int? RoleId { get; set; }
        public Role Role { get; set; }

        [Display(Name = "Количество голосов")]
        public int VotesCounter { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace WebApplication.Models.ViewModels
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Необходимо заполнить логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [Display(Name = "Подтвердите пароль")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Необходимо заполнить имя")]
        [Display(Name = "Имя")]
        public string FirstName { get; set; }

        public string Patronymic { get; set; }

        [Display(Name = "Фамилия")]
        public string LastName { get; set; }
    }
}

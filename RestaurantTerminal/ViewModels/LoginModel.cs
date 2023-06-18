using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Введите ваш логин !")]
        [StringLength(50, ErrorMessage = "Максимальная длина логина - 50 символов")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль !")]
        [StringLength(250, ErrorMessage = "Максимальная длина пароля - 250 символов")]
        public string Password { get; set; }

        public string Error { get; set; } = string.Empty;
    }
}

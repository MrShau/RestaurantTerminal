using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantTerminal.ViewModels
{
    public class AddEmployeeModel
    {
        [Required(ErrorMessage = "Введите логин !")]
        [StringLength(50, ErrorMessage = "Макс. количество символов - 50")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите пароль !")]
        [StringLength(250, ErrorMessage = "Макс. количество символов - 250")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Введите имя !")]
        [StringLength(50, ErrorMessage = "Макс. количество символов - 50")]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Макс. количество символов - 50")]
        [Required(ErrorMessage = "Введите фамилию !")]
        public string LastName { get; set; }

        [StringLength(50, ErrorMessage = "Макс. количество символов - 50")]
        [Required(ErrorMessage = "Введите отчество !")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Выберите дату рождения !")]
        [DataType(DataType.Date)]
        public DateOnly DateOfBirth { get; set; }

        [Required(ErrorMessage = "Выберите должность !")]
        public string Role { get; set; }

        [Required(ErrorMessage = "Введите номер телефона !")]
        [StringLength(16, MinimumLength = 6, ErrorMessage = "Неверный формат номера")]
        public string PhoneNumber { get; set; }



        public string Error { get; set; } = string.Empty;
    }
}

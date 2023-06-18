using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantTerminal.ViewModels
{
    public class AddDishModel
    {
        [Required(ErrorMessage = "Введите название блюда !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Выберите фото блюда !")]
        public IFormFile Image { get; set; }

        [Required(ErrorMessage = "Введите цену блюда !")]
        public double Price { get; set; }

        [Required(ErrorMessage = "Введите себестоимость блюда !")]
        public double CostPrice { get; set; }

        [Required(ErrorMessage = "Выберите категорию блюда !")]
        public string Category { get; set; }

        [AllowNull]
        public string Error { get; set; } = string.Empty;

        [AllowNull]
        public List<string> Categories = new List<string>();
    }
}

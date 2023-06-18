using RestaurantTerminal.Models;

namespace RestaurantTerminal.ViewModels
{
    public class MenuModel
    {
        public int DishId { get; set; }
        public string DishName { get; set; }
        public double DishPrice { get; set; }
        public string DishImageUrl { get; set; }
        public int Count { get; set; }
        public string CategoryName { get; set; }
    }
}

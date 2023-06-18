namespace RestaurantTerminal.ViewModels
{
    public class CreateOrderModel
    {
        public int DishId { get; set; }
        public string Name { get; set; } = string.Empty;
        public double Price { get; set; } = 0;
        public int Count { get; set; } = 0;

    }
}

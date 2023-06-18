using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.Models
{
    public class OrderDish
    {
        [Key]
        public int Id { get; set; }

        public int DishId { get; set; }
        public Dish Dish { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; }
        public int Count { get; set; }

        public OrderDish() { }

        public OrderDish(Dish dish, int count, Order order)
        {
            Dish = dish;
            Count = count;
            Order = order;
        }
    }
}

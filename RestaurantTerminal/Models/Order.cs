using Newtonsoft.Json;
using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantTerminal.Models
{
    public class Order : IOrder
    {
        [Key]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public List<OrderDish> Dishes { get; set; } = new List<OrderDish>();
        public bool IsDone { get; set; }
        public bool IsCanceled { get; set; }
        public bool IsCompleted { get; set; }


        public Check Check { get; set; }


        public Order()
        {
            CreatedDate = DateTime.Now;
            IsDone = false;
            IsCanceled = false;
        }

        public double GetTotalPrice()
        {
            double result = 0;

            try
            {
                if (Dishes != null)
                {
                    if (Dishes.Count > 0 && Dishes[0].Dish != null) 
                    {
                        foreach (var item in Dishes)
                        {
                            result += item.Dish.Price * item.Count;
                        }
                    }
                    
                }
            }
            catch { }

            return result;
        }

        public int GetCountDishes()
        {
            int count = 0;

            foreach(var item in Dishes)
            {
                count += item.Count;
            }

            return count;
        }
    }
}

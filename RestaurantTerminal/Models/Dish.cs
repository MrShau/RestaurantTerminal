using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantTerminal.Models
{
    public class Dish : IDish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(250)]
        public string ImageUrl { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public double CostPrice { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set;}

        [Required]
        public int CountOrdered { get; set; }

        [AllowNull]
        public bool IsDeleted { get; set; }

        public List<OrderDish> OrderDishes { get; set; }

        public Dish()
        {
            CountOrdered = 0;
            CreatedDate = DateTime.Now;
        }
        public Dish(string name, string imageUrl, double price, double costPrice, Category category) : this()
        {
            Name = name;
            ImageUrl = imageUrl;
            Price = price;
            CostPrice = costPrice;
            Category = category;
        }

        public uint GetCountCreated()
        {
            return 0;
        }
    }
}

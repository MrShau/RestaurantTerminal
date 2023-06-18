using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public List<Dish> Dishes { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public Category()
        {
            Dishes = new List<Dish>();
            CreatedDate = DateTime.Now;
        }

        public Category(string name) : this() => Name = name;

    }
}

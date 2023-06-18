using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantTerminal.Models
{
    public class Check : ICheck
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [Required]
        public double TotalPrice { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public Check()
        {
            CreatedDate = DateTime.Now;
        }

        public Check(Order order, double totalPrice, Employee employee) : this()
        {
            Order = order;
            TotalPrice = totalPrice;
            Employee = employee;
        }

        public long GetProfit()
        {
            long profit = 0;


            return profit;
        }

        public string GetCreatedDate() => CreatedDate.ToString("dd.MM.yyyy HH:mm:ss");
    }
}

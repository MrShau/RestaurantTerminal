using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.Models
{
    public class Finance : IFinance
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public double Income { get; set; }

        [Required]
        public double Consumption { get; set; }

        [Required]
        public double Profit { get; set; }

        public Finance()
        {
            Income = 0;
            Consumption = 0;
            Profit = 0;
        }

        public void AddIncome(double income)
        {
            Income += income;
            UpdateProfit();
        }

        public void AddConsumption(double consumption)
        {
            Consumption += consumption;
            UpdateProfit();
        }

        public void UpdateProfit() => Profit = Income - Consumption;

    }
}

namespace RestaurantTerminal.ViewModels
{
    public class FinanceModel
    {
        public double Income { get; set; }
        public double Consumption { get; set; }
        public double Profit { get; set; }
        public int CountCreatedOrders { get; set; }
        public int CountCompletedOrders { get; set; }
        public int CountCanceledOrders { get; set; }
        public double AverageCheck { get; set; }
        public Dictionary<string, int> Dishes { get; set; } = new Dictionary<string, int>();
    }
}

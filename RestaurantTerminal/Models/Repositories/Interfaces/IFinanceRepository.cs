namespace RestaurantTerminal.Models.Repositories.Interfaces
{
    public interface IFinanceRepository
    {
        public double GetIncome(DateOnly start,  DateOnly end);
        public double GetConsumption(DateOnly start, DateOnly end);
    }
}

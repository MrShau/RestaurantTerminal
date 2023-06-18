using Microsoft.EntityFrameworkCore;
using RestaurantTerminal.Database;
using RestaurantTerminal.Models.Repositories.Interfaces;

namespace RestaurantTerminal.Models.Repositories
{
    public class FinanceRepository : IFinanceRepository
    {
        private readonly AppDbContext _db;

        public FinanceRepository(AppDbContext db)
        {
            _db = db;
        }

        public double GetIncome(DateOnly start, DateOnly end)
        {
            double result = 0;

            var checks = _db.Checks.Where(c => !c.Order.IsCanceled && (c.Order.CreatedDate >= start.ToDateTime(TimeOnly.MinValue) && c.Order.CreatedDate <= end.ToDateTime(TimeOnly.MinValue)));

            foreach (var check in checks)
                result += check.TotalPrice;

            return result;
        }

        public double GetConsumption(DateOnly start, DateOnly end)
        {
            double result = 0;

            var checks = _db.Checks.Include(c => c.Order.Dishes).ThenInclude(d => d.Dish).Where(c => !c.Order.IsCanceled && (c.Order.CreatedDate >= start.ToDateTime(TimeOnly.MinValue) && c.Order.CreatedDate <= end.ToDateTime(TimeOnly.MinValue)));

            foreach (var check in checks)
                foreach (var dish in check.Order.Dishes)
                    result += dish.Dish.CostPrice * dish.Count;

            return result;
        }
    }
}

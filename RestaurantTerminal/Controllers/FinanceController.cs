using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTerminal.Database;
using RestaurantTerminal.Models.Repositories.Interfaces;
using RestaurantTerminal.ViewModels;

namespace RestaurantTerminal.Controllers
{
    public class FinanceController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IFinanceRepository _rep;

        public FinanceController(AppDbContext db, IFinanceRepository rep)
        {
            _db = db;
            _rep = rep;
        }

        private async Task UpdateFinance()
        {
            var finance = await _db.Finances.FirstOrDefaultAsync(f => f.Id == 1);
            try
            {

                foreach (var check in await _db.Checks.Include(c => c.Order.Dishes).ThenInclude(d => d.Dish).ToListAsync())
                {
                    if (!check.Order.IsCanceled)
                    {
                        double income = 0;
                        double cons = 0;
                        foreach (var dish in check.Order.Dishes)
                        {
                            income += dish.Dish.Price;
                            cons += dish.Dish.CostPrice;
                        }

                        finance.Income = income;
                        finance.Consumption = cons;
                        finance.Profit = income - cons;
                    }
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Index(string? type)
        {
            FinanceModel model = new FinanceModel();

            try
            {
                await UpdateFinance();
                ViewBag.Categories = await _db.Categories.ToListAsync();
                ViewBag.ActiveTab = 1;
                var finance = await _db.Finances.FirstAsync();
                if (finance != null)
                {
                    if (string.IsNullOrEmpty(type)) type = "all";

                    switch (type.ToLower())
                    {
                        case "today":
                            ViewBag.ActiveTab = 2;
                            model.Income = _rep.GetIncome(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
                            model.Consumption = _rep.GetConsumption(DateOnly.FromDateTime(DateTime.Today), DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
                            model.Profit = model.Income - model.Consumption;
                            model.CountCreatedOrders = _db.Orders.Where(o => o.CreatedDate >= DateTime.Today && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();
                            model.CountCanceledOrders = _db.Orders.Where(o => o.IsCanceled && o.CreatedDate >= DateTime.Today && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();
                            model.CountCompletedOrders = _db.Orders.Where(o => o.IsCompleted && o.CreatedDate >= DateTime.Today && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();

                            double averageCheck = 0;

                            foreach (var order in _db.Checks.Where(c => c.CreatedDate >= DateTime.Today && c.CreatedDate <= DateTime.Today.AddDays(1)))
                                averageCheck += order.TotalPrice;

                            model.AverageCheck = averageCheck / _db.Checks.Where(c => c.CreatedDate >= DateTime.Today && c.CreatedDate <= DateTime.Today.AddDays(1)).Count();


                            break;

                        case "week":
                            ViewBag.ActiveTab = 3;
                            model.Income = _rep.GetIncome(DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
                            model.Consumption = _rep.GetConsumption(DateOnly.FromDateTime(DateTime.Today.AddDays(-7)), DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
                            model.Profit = model.Income - model.Consumption;
                            model.CountCreatedOrders = _db.Orders.Where(o => o.CreatedDate >= DateTime.Today.AddDays(-7) && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();
                            model.CountCanceledOrders = _db.Orders.Where(o => o.IsCanceled && o.CreatedDate >= DateTime.Today.AddDays(-7) && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();
                            model.CountCompletedOrders = _db.Orders.Where(o => o.IsCompleted && o.CreatedDate >= DateTime.Today.AddDays(-7) && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();

                            double average3 = 0;

                            foreach (var order in _db.Checks.Where(c => c.CreatedDate >= DateTime.Today.AddDays(-7) && c.CreatedDate <= DateTime.Today.AddDays(1)))
                                average3 += order.TotalPrice;

                            model.AverageCheck = average3 / _db.Checks.Where(c => c.CreatedDate >= DateTime.Today.AddDays(-7) && c.CreatedDate <= DateTime.Today.AddDays(1)).Count();


                            break;

                        case "month":
                            ViewBag.ActiveTab = 4;
                            model.Income = _rep.GetIncome(DateOnly.FromDateTime(DateTime.Today.AddMonths(-1)), DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
                            model.Consumption = _rep.GetConsumption(DateOnly.FromDateTime(DateTime.Today).AddMonths(-1), DateOnly.FromDateTime(DateTime.Today.AddDays(1)));
                            model.Profit = model.Income - model.Consumption;
                            model.CountCreatedOrders = _db.Orders.Where(o => o.CreatedDate >= DateTime.Today.AddMonths(-1) && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();
                            model.CountCanceledOrders = _db.Orders.Where(o => o.IsCanceled && o.CreatedDate >= DateTime.Today.AddMonths(-1) && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();
                            model.CountCompletedOrders = _db.Orders.Where(o => o.IsCompleted && o.CreatedDate >= DateTime.Today.AddMonths(-1) && o.CreatedDate <= DateTime.Today.AddDays(1)).Count();

                            double average2 = 0;

                            foreach (var order in _db.Checks.Where(c => c.CreatedDate >= DateTime.Today.AddMonths(-1) && c.CreatedDate <= DateTime.Today.AddDays(1)))
                                average2 += order.TotalPrice;

                            model.AverageCheck = average2 / _db.Checks.Where(c => c.CreatedDate >= DateTime.Today.AddMonths(-1) && c.CreatedDate <= DateTime.Today.AddDays(1)).Count();


                            break;

                        default:
                            ViewBag.ActiveTab = 1;
                            model.Income = _rep.GetIncome(DateOnly.MinValue, DateOnly.MaxValue);
                            model.Consumption = _rep.GetConsumption(DateOnly.MinValue, DateOnly.MaxValue);
                            model.Profit = model.Income - model.Consumption;
                            model.CountCreatedOrders = _db.Orders.Count();
                            model.CountCanceledOrders = _db.Orders.Where(o => o.IsCanceled).Count();
                            model.CountCompletedOrders = _db.Orders.Where(o => o.IsCompleted).Count();

                            double average = 0;

                            foreach(var order in _db.Checks)
                                average += order.TotalPrice;

                            model.AverageCheck = average / _db.Checks.Count();

                            break;
                    }
                    
                    foreach(var dish in _db.Dishes.OrderByDescending(d => d.CountOrdered))
                    {
                        model.Dishes.Add(dish.Name, dish.CountOrdered);
                    }
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return View(model);
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTerminal.Database;
using RestaurantTerminal.Extensions;
using RestaurantTerminal.Models;
using RestaurantTerminal.ViewModels;
using System.Security.Claims;

namespace RestaurantTerminal.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _db;

        public OrderController(AppDbContext db) => _db = db;

        [HttpGet]
        [Authorize(Roles = "ADMIN, MANAGER, CASHIER")]
        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();
            List<CreateOrderModel> model = new List<CreateOrderModel>();
            bool isEmpty = true;
            try
            {
                foreach (var dish in await _db.Dishes.ToListAsync())
                {
                    if (HttpContext.Session.Keys.Contains($"dish_{dish.Id}"))
                    {
                        ApplyDishModel sData = HttpContext.Session.Get<ApplyDishModel>($"dish_{dish.Id}");
                        isEmpty = false;
                        var d = await _db.Dishes.FirstOrDefaultAsync(e => e.Id == sData.Id);
                        model.Add(new CreateOrderModel { Name = d.Name, Count = sData.Count, Price = d.Price, DishId = d.Id });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return isEmpty ? Redirect("/Home/Index") : View(model);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER, CASHIER")]
        public void AddDish(int count, int dish_id)
        {
            if (count > 0)
                HttpContext.Session.Set($"dish_{dish_id}", new ApplyDishModel { Id = dish_id, Count = count });
            else if (HttpContext.Session.Keys.Contains($"dish_{dish_id}"))
                HttpContext.Session.Remove($"dish_{dish_id}");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER, CASHIER")]
        public async Task<IActionResult> Create(int a)
        {
            try
            {
                bool isNull = true;

                for (int i = 0; i <= _db.Dishes.ToList().Last().Id; i++)
                {
                    if (HttpContext.Session.Keys.Contains($"dish_{i}"))
                    {
                        isNull = false;
                        break;
                    }
                }

                if (!isNull)
                {
                    var order = _db.Orders.Add(new Order()).Entity;
                    for (int i = 0; i <= _db.Dishes.ToList().Last().Id; i++)
                    {
                        if (HttpContext.Session.Keys.Contains($"dish_{i}"))
                        {
                            ApplyDishModel sData = HttpContext.Session.Get<ApplyDishModel>($"dish_{i}");
                            await _db.OrderDishes.AddAsync(new OrderDish(await _db.Dishes.FirstOrDefaultAsync(d => d.Id == sData.Id), sData.Count, order));
                            _db.Dishes.FirstOrDefault(d => d.Id == i).CountOrdered += sData.Count;
                            await _db.SaveChangesAsync();
                            HttpContext.Session.Remove($"dish_{i}");
                        }
                    }
                    await _db.SaveChangesAsync();
                    await _db.Checks.AddAsync(new Check(order, order.GetTotalPrice(), await _db.Employees.FirstOrDefaultAsync(e => e.Id == int.Parse(User.FindFirstValue(ClaimTypes.Sid)))));
                    await _db.SaveChangesAsync();
                }

                ViewBag.Categories = await _db.Categories.ToListAsync();

            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }


            return Redirect("/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> List(string? type)
        {
            try
            {
                var employee = _db.Employees.Include(e => e.ActivityStatistics).FirstOrDefault(e => e.Id == int.Parse(User.FindFirst(ClaimTypes.Sid).Value));

                if (employee != null)
                {
                    employee.ActivityStatistics.Last().EndDate = DateTime.Now.AddMinutes(1);
                    _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            List<Order> model = new List<Order>();
            int activeTab = 1;
            try
            {
                if (type != null)
                {
                    switch (type.ToLower())
                    {
                        case "canceled":
                            model = _db.Orders.Include(o => o.Dishes).ThenInclude(d => d.Dish).Include(o => o.Check).Where(o => o.IsCanceled).ToList();
                            activeTab = 4;
                            break;

                        case "ready":
                            model = _db.Orders.Include(o => o.Dishes).ThenInclude(d => d.Dish).Include(o => o.Check).Where(o => o.IsDone && !o.IsCompleted).ToList();
                            activeTab = 2;
                            break;

                        case "completed":
                            model = _db.Orders.Include(o => o.Dishes).ThenInclude(d => d.Dish).Include(o => o.Check).Where(o => !o.IsCanceled && o.IsCompleted).ToList();
                            activeTab = 3;
                            break;

                        default:
                            model = _db.Orders.Include(o => o.Dishes).ThenInclude(d => d.Dish).Include(o => o.Check).Where(o => !o.IsCanceled && !o.IsDone).ToList();
                            break;
                    }
                }
                else
                {
                    model = _db.Orders.Include(o => o.Dishes).ThenInclude(d => d.Dish).Include(o => o.Check).Where(o => !o.IsCanceled && !o.IsDone).ToList();
                }
                ViewBag.Categories = _db.Categories.ToList();

                ViewBag.OrdersCanceledCount = await _db.Orders.Where(d => d.IsCanceled).CountAsync();
                ViewBag.OrdersCount = await _db.Orders.Where(o => !o.IsCanceled && !o.IsDone).CountAsync();
                ViewBag.OrdersReadyCount = await _db.Orders.Where(o => o.IsDone && !o.IsCompleted).CountAsync();
                ViewBag.OrdersCompletedCount = await _db.Orders.Where(o => !o.IsCanceled && o.IsCompleted).CountAsync();

            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.Message);
            }
            ViewBag.ActiveTab = activeTab;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Cancellation(int order_id)
        {
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == order_id);
                if (order != null)
                {
                    order.IsCanceled = true;
                    await _db.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return Redirect(Request.Headers.Referer.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Complete(int order_id)
        {
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == order_id);
                if (order != null)
                {
                    order.IsCompleted = true;
                    await _db.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return Redirect(Request.Headers.Referer.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Ready(int order_id)
        {
            try
            {
                var order = await _db.Orders.FirstOrDefaultAsync(o => o.Id == order_id);
                if (order != null)
                {
                    order.IsDone = true;
                    await _db.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return Redirect(Request.Headers.Referer.ToString());
        }

        [HttpPost]
        public async Task<IActionResult> Clear()
        {
            try
            {
                for (int i = 0; i <= _db.Dishes.ToList().Last().Id; i++)
                    if (HttpContext.Session.Keys.Contains($"dish_{i}"))
                        HttpContext.Session.Remove($"dish_{i}");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return Redirect("/Home/Index");
        }

        [HttpGet]
        public async Task<IActionResult> InfoTable()
        {
            ViewBag.Categories = await _db.Categories.ToListAsync();

            var model = new Dictionary<int, bool>();

            ViewBag.OrdersReadyCount = await _db.Orders.Where(o => o.IsDone && !o.IsCompleted).CountAsync();
            ViewBag.OrdersCount = await _db.Orders.Where(o => !o.IsDone && !o.IsCanceled && !o.IsCompleted).CountAsync();

            foreach (var order in _db.Orders.Where(o => !o.IsCanceled && !o.IsCompleted).OrderByDescending(o => o.IsDone))
            {
                model.Add(order.Id, order.IsDone);
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CheckNewOrder(int readycount = 0, int count = 0, int canceledcount = 0, int completedcount = 0)
        {

            if (
                (await _db.Orders.Where(o => o.IsDone && !o.IsCompleted).CountAsync() > readycount && readycount > 0) ||
                (await _db.Orders.Where(o => !o.IsDone && !o.IsCanceled && !o.IsCompleted).CountAsync() > count && count > 0) ||
                (await _db.Orders.Where(d => d.IsCanceled).CountAsync() > canceledcount && canceledcount > 0) ||
                (await _db.Orders.Where(o => !o.IsCanceled && o.IsCompleted).CountAsync() > completedcount && completedcount > 0)
                )
            {
                return Ok(new
                {
                    new_order = true
                });
            }
            return Ok(new
            {
                new_order = false
            });
        }
    }
}

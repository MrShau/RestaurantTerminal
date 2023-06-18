using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTerminal.Database;
using RestaurantTerminal.Extensions;
using RestaurantTerminal.Models;
using RestaurantTerminal.Models.Interfaces;
using RestaurantTerminal.ViewModels;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Security.Claims;

namespace RestaurantTerminal.Controllers
{
    struct ApplyDishModel
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }

    [Authorize]
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public HomeController(AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet]
        public  IActionResult Index(int? category_id)
        {
            if (User.IsInRole("COOK")) return Redirect("/Home/ListOrders?type=actives");
            try
            {
                var employee =   _db.Employees.Include(e => e.ActivityStatistics).FirstOrDefault(e => e.Id == int.Parse(User.FindFirst(ClaimTypes.Sid).Value));
                if (employee != null)
                {
                    employee.ActivityStatistics.Last().EndDate = DateTime.Now;
                    _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            ViewBag.HttpContext = HttpContext;
            ViewBag.CurrentAction = "Index";
            ViewBag.CurrentController = "Home";
            ViewBag.Title = "Меню";


            var model = new List<MenuModel>();

            try
            {
                ViewBag.Categories = _db.Categories.ToList();

                if (category_id == null || category_id == 0)
                {
                    foreach (var item in _db.Dishes.Include(d => d.Category).OrderByDescending(d => d.CountOrdered))
                    {
                        int count = 0;
                        if (HttpContext.Session.Keys.Contains($"dish_{item.Id}"))
                        {
                            count = HttpContext.Session.Get<ApplyDishModel>($"dish_{item.Id}").Count;
                        }

                        model.Add(new MenuModel
                        {
                            DishId = item.Id,
                            DishName = item.Name,
                            DishImageUrl = item.ImageUrl,
                            DishPrice = item.Price,
                            CategoryName = item.Category.Name,
                            Count = count

                        });
                    }

                }
                else
                {
                    foreach (var item in _db.Dishes.Include(d => d.Category).Where(d => d.Category.Id == category_id).ToList())
                    {
                        int count = 0;
                        if (HttpContext.Session.Keys.Contains($"dish_{item.Id}"))
                        {
                            count = HttpContext.Session.Get<ApplyDishModel>($"dish_{item.Id}").Count;
                        }

                        model.Add(new MenuModel
                        {
                            DishId = item.Id,
                            DishName = item.Name,
                            DishImageUrl = item.ImageUrl,
                            DishPrice = item.Price,
                            CategoryName = item.Category.Name,
                            Count = count

                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> AddCategory(string category_name)
        {
            if (string.IsNullOrEmpty(category_name) || string.IsNullOrWhiteSpace(category_name)) return Redirect(Request.Headers["Referer"].ToString() ?? "/Home/Index");

            try
            {
                if (await _db.Categories.FirstOrDefaultAsync(c => c.Name.ToLower() == category_name.ToLower()) == null)
                {
                    var category = await _db.Categories.AddAsync(new Category(category_name));
                    await _db.SaveChangesAsync();
                }

            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return Redirect(Request.Headers["Referer"].ToString() ?? "/Home/Index");

        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult Employees()
        {
            EmployeesModel model = new EmployeesModel();

            try
            {
                foreach(var item in _db.Employees.Include(e => e.Person).Include(e => e.Role).ToList())
                {
                    model.Employees.Add(item);
                }
                ViewBag.Categories = _db.Categories.ToList();
			}
			catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(model);
        }
        
    }
}
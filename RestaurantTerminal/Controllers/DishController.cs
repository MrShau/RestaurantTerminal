using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantTerminal.Models;
using RestaurantTerminal.ViewModels;
using System.Drawing.Imaging;
using System.Drawing;
using RestaurantTerminal.Database;
using Microsoft.EntityFrameworkCore;

namespace RestaurantTerminal.Controllers
{
    public class DishController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _environment;

        public DishController (AppDbContext db, IWebHostEnvironment environment)
        {
            _db = db;
            _environment = environment;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult Add()
        {
            AddDishModel model = new AddDishModel();

            try
            {
                foreach (var item in _db.Categories.ToList())
                {
                    model.Categories.Add(item.Name);
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
        public async Task<IActionResult> Add(AddDishModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                if (await _db.Dishes.FirstOrDefaultAsync(d => d.Name.ToLower() == model.Name.ToLower()) != null)
                {
                    model.Error = "Такое блюдо уже есть в меню";
                    return View(model);
                }

                string path = $"img/{Guid.NewGuid().ToString()}_{model.Image.FileName}";

                using (var fileStream = new FileStream(_environment.WebRootPath + "/" + path, FileMode.Create))
                {
                    var image = System.Drawing.Image.FromStream(model.Image.OpenReadStream());
                    var resized = new Bitmap(image, new Size(400, 250));

                    resized.Save(fileStream, ImageFormat.Jpeg);
                    await model.Image.CopyToAsync(fileStream);
                }

                await _db.Dishes.AddAsync(new Dish(model.Name, path, model.Price, model.CostPrice, _db.Categories.FirstOrDefault(c => c.Name.ToLower() == model.Category.ToLower())));
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Edit(int dish_id)
        {
            EditDishModel model = new EditDishModel();
            try
            {

                var dish = await _db.Dishes.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == dish_id);
                model.Id = dish.Id;
                model.Category = dish.Category.Name;
                model.Price = dish.Price;
                model.CostPrice = dish.CostPrice;
                model.Name = dish.Name;

                foreach (var item in _db.Categories.ToList())
                {
                    model.Categories.Add(item.Name);
                }
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Edit(EditDishModel model)
        {
            try
            {
                var dish = await _db.Dishes.Include(d => d.Category).FirstOrDefaultAsync(d => d.Id == model.Id);

                dish.Name = model.Name;
                dish.Price = model.Price;
                dish.CostPrice = model.CostPrice;

                var category = await _db.Categories.FirstOrDefaultAsync(c => c.Name == model.Category);

                if (dish.Category.Name != category.Name)
                {
                    dish.Category = category;
                    dish.CategoryId = category.Id;
                }

                if (model.Image != null)
                {


                    string path = $"img/{Guid.NewGuid().ToString()}_{model.Image.FileName}";

                    using (var fileStream = new FileStream(_environment.WebRootPath + "/" + path, FileMode.Create))
                    {
                        var image = System.Drawing.Image.FromStream(model.Image.OpenReadStream());
                        var resized = new Bitmap(image, new Size(400, 250));

                        resized.Save(fileStream, ImageFormat.Jpeg);
                        await model.Image.CopyToAsync(fileStream);
                    }
                    string oldPath = _environment.WebRootPath + "/" + dish.ImageUrl;
                    if (System.IO.File.Exists(oldPath))
                    {
                        System.IO.File.Delete(oldPath);
                    }
                    dish.ImageUrl = path;
                }
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
                foreach (var item in _db.Categories.ToList())
                {
                    model.Categories.Add(item.Name);
                }
                return View(model);
            }

            return Redirect("/Home/Index");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> Delete(int dish_id)
        {
            try
            {
                _db.Dishes.FirstOrDefault(d => d.Id == dish_id).IsDeleted = true;
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                await Console.Out.WriteLineAsync(e.Message);
            }

            return Redirect(Request.Headers.Referer.ToString() ?? "/Home/Index");
        }

    }
}

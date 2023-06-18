using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTerminal.Database;
using RestaurantTerminal.Models;
using RestaurantTerminal.ViewModels;
using System.Security.Claims;

namespace RestaurantTerminal.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _db;

        private bool IsEmpty(string text) => string.IsNullOrEmpty(text) || string.IsNullOrWhiteSpace(text);

        public AuthController(AppDbContext db)
        {
            _db = db;
        }

        private void InitDb()
        {
            _db.Roles.AddRange(new Role("ADMIN"), new Role("MANAGER"), new Role("CASHIER"), new Role("COOK"));
            _db.SaveChanges();
            var person = _db.People.Add(new Person("ads", "ads", "ads", DateTime.Now)).Entity;
            _db.SaveChanges();
            _db.Employees.Add(new Employee(person, "admin", "2711", _db.Roles.First(), "+79280007722"));
            _db.SaveChanges();
            _db.Finances.Add(new Finance());
            _db.SaveChanges();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            ViewBag.Title = "Авторизация";
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                Employee? employee = await _db.Employees.Include(e => e.Role).Include(e => e.Person).FirstOrDefaultAsync(e => e.Login == model.Login.ToLower());
                if (employee == null)
                {
                    model.Error = "Неправильный логин !";
                    return View(model);
                }

                if (!employee.CheckPassword(model.Password))
                {
                    model.Error = "Неверный пароль !";
                    return View(model);
                }

                await AuthenticateAsync(employee.Login, employee.Role.Name, employee.Person.FirstName, employee.Person.LastName, employee.Id, employee.RoleId);
                _db.ActivityStatistics.Add(new ActivityStatistics(employee));
                await _db.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            model.Error = "Ошибка сервера";
            return View(model);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult AddEmployee() => View();

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> AddEmployee(AddEmployeeModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                if (await _db.Employees.FirstOrDefaultAsync(e => e.Login == model.Login.ToLower()) != null)
                {
                    model.Error = "Логин занят";
                    return View(model);
                }

                Person person = _db.People.Add(new Person(model.FirstName, model.LastName, model.Patronymic, model.DateOfBirth.ToDateTime(new TimeOnly(0)))).Entity;
                _db.Employees.Add(new Employee(person, model.Login.ToLower(), model.Password, _db.Roles.FirstOrDefault(r => r.Name == model.Role), model.PhoneNumber));

                await _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public async Task<IActionResult> EditEmployee(EmployeesModel model)
        {
            try
            {
                var employee = await _db.Employees.Include(e => e.Person).Include(e => e.Role).FirstOrDefaultAsync(e => e.Id == model.Id);
                if (employee == null)
                {
                    return Redirect(Request.Headers["Referer"].ToString() ?? "/Home/Index");
                }

                if (!IsEmpty(model.Login))
                    employee.Login = model.Login;

                if (!IsEmpty(model.Password))
                    employee.Password = model.Password;

                if (!IsEmpty(model.FirstName))
                    employee.Person.FirstName = model.FirstName;

                if (!IsEmpty(model.LastName))
                    employee.Person.LastName = model.LastName;

                if (!IsEmpty(model.Patronymic))
                    employee.Person.Patronymic = model.Patronymic;

                if (employee.Role.Name != model.Role)
                {
                    var role = await _db.Roles.FirstOrDefaultAsync(r => r.Name == model.Role);
                    if (role != null)
                    {
                        employee.Role = role;
                        employee.RoleId = role.Id;
                    }
                }

                await _db.SaveChangesAsync();
            }
            catch(Exception ex)
            {
                await Console.Out.WriteLineAsync(ex.Message);
            }

            return Redirect(Request.Headers["Referer"].ToString() ?? "/Home/Index");
        }

        /// <summary>
        /// Авторизовывает пользователя, создает cookie с данными пользователя
        /// </summary>
        private async Task AuthenticateAsync(string login, string roleName, string first_name, string last_name, int id, int roleId)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Actor, login),
                new Claim(ClaimTypes.Role, roleName.ToUpper()),
                new Claim(ClaimTypes.NameIdentifier, roleId.ToString()),
                new Claim(ClaimTypes.Name, first_name),
                new Claim(ClaimTypes.Surname, last_name),
                new Claim(ClaimTypes.Sid, id.ToString())
            };

            var claimsIdenties = new ClaimsIdentity(claims, "ApllicationCookie", ClaimTypes.Email, ClaimTypes.Role);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdenties));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            
            
            return Redirect("/Home/Index");
        }

    }
}

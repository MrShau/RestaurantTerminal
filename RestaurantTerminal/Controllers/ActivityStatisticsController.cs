using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantTerminal.Database;
using RestaurantTerminal.Models;
using OfficeOpenXml;

namespace RestaurantTerminal.Controllers
{
    public class ActivityStatisticsController : Controller
    {
        private readonly AppDbContext _db;

        public ActivityStatisticsController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN, MANAGER")]
        public IActionResult Index(int employee_id)
        {
            Employee? model = null;
            try
            {
                ViewBag.Categories = _db.Categories.ToList();
                model = _db.Employees.Include(e => e.ActivityStatistics).Include(e => e.Person).FirstOrDefault(e => e.Id == employee_id);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> ExportToExcel(int employee_id)
        {
            if (employee_id > 0)
            {
                    using (var package = new ExcelPackage())
                    {
                        // Создание нового листа
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                        // Заполнение ячеек данными
                        worksheet.Cells["A1"].Value = "Номер";
                        worksheet.Cells["B1"].Value = "Время входа";
                        worksheet.Cells["C1"].Value = "Время выхода";
                        worksheet.Cells["D1"].Value = "Длительность сессии";

                        var statistics = _db.Employees.Include(e => e.ActivityStatistics).Include(e => e.Person).FirstOrDefault(e => e.Id == employee_id)?.ActivityStatistics;

                        if (statistics != null)
                        {
                            for (int i = 0; i < statistics.Count; i++)
                            {
                                worksheet.Cells[$"A{i+2}"].Value = statistics[i].Id.ToString();
                                worksheet.Cells[$"B{i+2}"].Value = statistics[i].StartDateToString();
                                worksheet.Cells[$"C{i+2}"].Value = statistics[i].EndDateToString();
                                worksheet.Cells[$"D{i+2}"].Value = statistics[i].SessionDuration();
                            }
                        }

                        worksheet.Columns.AutoFit();

                        byte[] bytes = package.GetAsByteArray();
                        return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"Статистика{employee_id}.xlsx");

                    }
            }
            return Redirect("/Home/Employees");

        }

    }
}

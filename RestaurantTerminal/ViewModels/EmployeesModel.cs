using RestaurantTerminal.Models;
using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.ViewModels
{
    public class EmployeesModel
    {
        public List<Employee> Employees { get; set; } = new List<Employee>();
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Patronymic { get; set; }
        public string Role { get; set; }
    }
}

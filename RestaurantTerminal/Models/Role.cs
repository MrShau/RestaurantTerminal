using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.Models
{
    public class Role : IRole
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public List<Employee> Employees { get; set; }

        public Role()
        {
            Employees = new List<Employee>();
        }

        public Role(string name) : this() => Name = name;

        public int GetCountEmployees() => Employees.Count;

        public string NameToRussian()
        {
            string result = "";

            switch(Name)
            {
                case "ADMIN":
                    result = "Администратор";
                    break;
                case "MANAGER":
                    result = "Менеджер";
                    break;
                case "CASHIER":
                    result = "Кассир";
                    break;
                case "COOK":
                    result = "Повар";
                    break;
            }

            return result;
        }
    }
}

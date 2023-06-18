using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace RestaurantTerminal.Models
{
    public class Employee : IEmployee
    {
        [Key]
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        [Required]
        public int RoleId { get; set; }
        public Role Role { get; set; }

        [Required]
        [StringLength(100)]
        public string Login { get; set; }

        [Required]
        [StringLength(300)]
        public string Password { get; set; }

        [AllowNull]
        [StringLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set;}

        public List<Check> Checks { get; set; } = new List<Check>();
        public List<ActivityStatistics> ActivityStatistics { get; set; } = new List<ActivityStatistics>();

        public Employee() => CreatedDate = DateTime.Now;

        public Employee(Person person, string login, string password, Role role, string phoneNumber) : this()
        {
            Person = person;
            Login = login;
            Password = password;
            Role = role;
            PhoneNumber = phoneNumber;
        }

        public List<Check> GetCreatedChecks() => new List<Check>(Checks);
        public bool CheckPassword(string password) => password == Password;
    }
}

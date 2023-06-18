using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.Models
{
    public class Person : IPerson
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        public string Patronymic { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        public Employee Employee { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        public Person() => CreatedDate = DateTime.Now;

        public Person(string firstName, string lastName, string patronymic, DateTime dateOfBirth) : this()
        {
            FirstName = firstName.Replace(" ", "");
            LastName = lastName.Replace(" ", "");
            Patronymic = patronymic.Replace(" ", "");
            DateOfBirth = dateOfBirth;
        }

        public string GetFullName() => $"{LastName} {FirstName} {Patronymic}";
        public string GetInitials() => $"{FirstName[0]}. {Patronymic[0]}. {LastName}";
        public string GetDateOfBirth() => DateOfBirth.ToString("dd.MM.yyyy");
    }
}

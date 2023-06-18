using RestaurantTerminal.Models.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RestaurantTerminal.Models
{
    public class ActivityStatistics : IActivityStatistics
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        public ActivityStatistics()
        {
            StartDate = DateTime.Now;
            EndDate = DateTime.Now.AddMinutes(5);
        }

        public ActivityStatistics(Employee employee) : this()
        {
            Employee = employee;
        }

        public string StartDateToString() => StartDate.ToString("dd.MM.yyyy HH:mm");
        public string EndDateToString() => EndDate.ToString("dd.MM.yyyy HH:mm");
        public string SessionDuration()
        {
            string result = "";

            try
            {
                result = new DateTime(EndDate.Ticks - StartDate.Ticks).ToString("HH ч. mm м. ss с.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }
    }
}

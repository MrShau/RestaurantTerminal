namespace RestaurantTerminal.Models.Interfaces
{
    public interface IActivityStatistics
    {
        /// <summary>
        /// Дата и время захода в аккаунт
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// Дата и время выхода из аккаунта
        /// </summary>
        public DateTime EndDate { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }

        /// <returns>Дата и время входа на сайт в формате дд.мм.гггг чч.мм</returns>
        public string StartDateToString();
        /// <returns>Дата и время выхода из сайта в формате дд.мм.гггг чч.мм</returns>
        public string EndDateToString();
        /// <returns>Длительность сессии в формате чч.мм</returns>
        public string SessionDuration();

    }
}

namespace RestaurantTerminal.Models.Interfaces
{
    public interface IPerson
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Имя
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Отчество
        /// </summary>
        public string Patronymic { get; set; }

        /// <summary>
        /// Дата рождения
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Идентификатор поля в таблице Employee
        /// </summary>
        public Employee Employee { get; set; }

        /// <summary>
        /// Дата добалвения записи
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <returns>Инициалы человека</returns>
        public string GetInitials();

        /// <returns>Полное ФИО человека</returns>
        public string GetFullName();

        /// <returns>Дату рождения человека в формате дд.мм.гггг </returns>
        public string GetDateOfBirth();
    }
}

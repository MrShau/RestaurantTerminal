namespace RestaurantTerminal.Models.Interfaces
{
    public interface IRole
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Список работников с данной ролью
        /// </summary>
        public List<Employee> Employees { get; set; }

        /// <summary>
        /// Переводит название роли на русский
        /// </summary>
        /// <returns>Русское название роли </returns>
        public string NameToRussian();

        /// <returns>Возвращает количество работников с данной ролью</returns>
        public int GetCountEmployees();
    }
}

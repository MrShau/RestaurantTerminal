
namespace RestaurantTerminal.Models.Interfaces
{
    public interface IEmployee
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        public int PersonId { get; set; }
        public Person Person { get; set; }

        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public int RoleId { get; set; }
        public Role Role { get; set; }
        public string Login { get; set; }
        
        /// <summary>
        /// Хэшированный пароль
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Дата добавления работника
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <returns>Созданные чеки</returns>
        public List<Check> GetCreatedChecks();

        /// <summary>
        /// Проверяет правильность введенного пароля
        /// </summary>
        /// <param name="password">Пароль, который надо проверить</param>
        /// <returns>false - если пароль не совпадает<br/>true - если введен правильный пароль</returns>
        public bool CheckPassword(string password);
    }
}

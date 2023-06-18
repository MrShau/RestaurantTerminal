namespace RestaurantTerminal.Models.Interfaces
{
    public interface ICategory
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
        /// Дата добавления
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Список блюд
        /// </summary>
        public List<Dish> Dishes { get; set; }

    }
}

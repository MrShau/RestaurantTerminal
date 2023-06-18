namespace RestaurantTerminal.Models.Interfaces
{
    public interface IOrder
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Дата создания заказа
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Список блюд
        /// </summary>
        public List<OrderDish> Dishes { get; set; }

        public bool IsDone { get; set; }

    }
}

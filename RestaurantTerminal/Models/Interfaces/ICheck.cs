namespace RestaurantTerminal.Models.Interfaces
{
    public interface ICheck
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Заказ
        /// </summary>
        public Order Order { get; set; }
        /// <summary>
        /// Итоговый счет (сумма)
        /// </summary>
        public double TotalPrice { get; set; }

        /// <summary>
        /// Дата и время создания чека
        /// </summary>
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Идентификатор работника, создавший чек
        /// </summary>
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; }


        /// <returns>Значение прибыли</returns>
        public long GetProfit();

        /// <returns>Дату и время в формате дд.мм.гггг чч:мм:сс</returns>
        public string GetCreatedDate();
    }
}

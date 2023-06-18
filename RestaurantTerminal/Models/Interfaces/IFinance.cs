namespace RestaurantTerminal.Models.Interfaces
{
    public interface IFinance
    {
        /// <summary>
        /// Уникальный идентификатор
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Весь доход
        /// </summary>
        public double Income { get; set; }
        
        /// <summary>
        /// Все расходы
        /// </summary>
        public double Consumption { get; set; }

        /// <summary>
        /// Прибыль
        /// </summary>
        public double Profit { get; set; }

        /// <summary>
        /// Добавляет доход
        /// </summary>
        /// <param name="income">Значение дохода</param>
        public void AddIncome(double income);

        /// <summary>
        /// Добавляет расход
        /// </summary>
        /// <param name="consumption">Значение расхода</param>
        public void AddConsumption(double consumption);

        /// <summary>
        /// Обновляет значение прибыли
        /// </summary>
        protected void UpdateProfit();


    }
}

namespace RestaurantTerminal.Models.Interfaces
{
    public interface IDish
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
        /// Путь до изображения (путь без домена и порта)
        /// </summary>
        public string ImageUrl { get; set; }

        /// <summary>
        /// Цена блюда
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// Себестоимость
        /// </summary>
        public double CostPrice { get; set; }

        /// <summary>
        /// Идентификатор категории блюда
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// Модель категории блюда
        /// </summary>
        public Category Category { get; set; }

        /// <summary>
        /// Дата и время добавления блюда в меню
        /// </summary>
        public DateTime CreatedDate { get; set; }

        ///
        /// <returns>Количество сделанных блюд</returns>
        /// 
        public uint GetCountCreated();
    }
}

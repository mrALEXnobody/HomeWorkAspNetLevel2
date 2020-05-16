namespace WebStoreGusev.Models
{
    /// <summary>
    /// Класс автомобиля.
    /// </summary>
    public class CarViewModel
    {
        /// <summary>
        /// Артикул автомобиля.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Производитель автомобиля.
        /// </summary>
        public string Company { get; set; }

        /// <summary>
        /// Модель автомобиля.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Цвет автомобиля.
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Цена автомобиля.
        /// </summary>
        public double Price { get; set; }
    }
}

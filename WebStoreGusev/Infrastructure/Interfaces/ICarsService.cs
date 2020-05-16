using System.Collections.Generic;
using WebStoreGusev.Models;

namespace WebStoreGusev.Infrastructure.Interfaces
{
    public interface ICarsService
    {
        /// <summary>
        /// Получение списка автомобилей.
        /// </summary>
        /// <returns></returns>
        IEnumerable<CarViewModel> GetAll();

        /// <summary>
        /// Получение автомобиля по Id.
        /// </summary>
        /// <param name="id">Id автомобиля.</param>
        /// <returns></returns>
        CarViewModel GetById(int id);

        /// <summary>
        /// Сохранить изменения.
        /// </summary>
        void Commit();

        /// <summary>
        /// Добавить новый автомобиль.
        /// </summary>
        /// <param name="model"></param>
        void AddNew(CarViewModel model);

        /// <summary>
        /// Удалить автомобиль.
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
    }
}

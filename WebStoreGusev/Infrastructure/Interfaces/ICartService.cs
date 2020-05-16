using WebStoreGusev.Models;

namespace WebStoreGusev.Infrastructure.Interfaces
{
    public interface ICartService
    {
        /// <summary>
        /// Уменьшить количество предметов в корзине.
        /// </summary>
        /// <param name="id"></param>
        void DecrementFromCart(int id);

        /// <summary>
        /// Удалить из корзины.
        /// </summary>
        /// <param name="id"></param>
        void RemoveFromCart(int id);

        /// <summary>
        /// Удалить всё из корзины.
        /// </summary>
        void RemoveAll();

        /// <summary>
        /// Добавить в корзину.
        /// </summary>
        /// <param name="id"></param>
        void AddToCart(int id);

        CartViewModel TransformCart();
    }
}

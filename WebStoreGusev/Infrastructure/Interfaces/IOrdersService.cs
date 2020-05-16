using System.Collections.Generic;
using WebStoreGusev.DomainNew.Entities;
using WebStoreGusev.Models;

namespace WebStoreGusev.Infrastructure.Interfaces
{
    public interface IOrdersService
    {
        IEnumerable<Order> GetUserOrders(string userName);
        Order GetOrderById(int id);
        Order CreateOrder(OrderViewModel orderModel, CartViewModel transformCart, string userName);
    }
}

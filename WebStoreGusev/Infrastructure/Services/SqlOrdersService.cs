using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStoreGusev.DAL;
using WebStoreGusev.DomainNew.Entities;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Models;

namespace WebStoreGusev.Infrastructure.Services
{
    public class SqlOrdersService : IOrdersService
    {
        private readonly WebStoreContext context;
        private readonly UserManager<User> userManager;

        public SqlOrdersService(WebStoreContext context, UserManager<User> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        public Order CreateOrder(OrderViewModel orderModel, CartViewModel transformCart, string userName)
        {
            var user = userManager.FindByNameAsync(userName).Result;

            using (var transaction = context.Database.BeginTransaction())
            {
                var order = new Order()
                {
                    Address = orderModel.Address,
                    Name = orderModel.Name,
                    Date = DateTime.Now,
                    Phone = orderModel.Phone,
                    User = user
                };

                context.Orders.Add(order);

                foreach (var item in transformCart.Items)
                {
                    var productVm = item.Key;
                    var product = context.Products.FirstOrDefault(p => p.Id.Equals(productVm.Id));

                    if (product == null)
                        throw new InvalidOperationException("Продукт не найден в базе");

                    var orderItem = new OrderItem()
                    {
                        Price = product.Price,
                        Quantity = item.Value,

                        Order = order,
                        Product = product
                    };

                    context.OrderItems.Add(orderItem);
                }

                context.SaveChanges();

                transaction.Commit();

                return order;
            }
        }

        public Order GetOrderById(int id)
        {
            return context.Orders
                 .Include(x => x.User)
                 .Include(x => x.OrderItems)
                 .FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<Order> GetUserOrders(string userName)
        {
            return context.Orders
                .Include(x => x.User)   // 1 вариант
                .Include("OrderItems")  // 2 вариант
                .Where(x => x.User.UserName == userName)
                .ToList();
        }
    }
}

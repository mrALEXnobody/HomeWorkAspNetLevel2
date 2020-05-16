using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using WebStoreGusev.DomainNew;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Models;

namespace WebStoreGusev.Infrastructure.Services
{
    public class CookieCartService : ICartService
    {
        private readonly IProductService productService;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly string cartName;

        private Cart Cart
        {
            get
            {
                var cookie = httpContextAccessor
                                .HttpContext
                                .Request
                                .Cookies[cartName];
                string json = string.Empty;
                Cart cart = null;

                if (cookie == null)
                {
                    cart = new Cart { Items = new List<CartItem>() };
                    json = JsonConvert.SerializeObject(cart);

                    httpContextAccessor
                        .HttpContext
                        .Response
                        .Cookies
                        .Append(
                            cartName,
                            json,
                            new CookieOptions
                            {
                                Expires = DateTime.Now.AddDays(1)
                            });
                    return cart;
                }

                json = cookie;
                cart = JsonConvert.DeserializeObject<Cart>(json);

                httpContextAccessor
                    .HttpContext
                    .Response
                    .Cookies
                    .Delete(cartName);

                httpContextAccessor
                   .HttpContext
                   .Response
                   .Cookies
                   .Append(
                        cartName,
                        json,
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(1)
                        });

                return cart;
            }
            set
            {
                var json = JsonConvert.SerializeObject(value);

                httpContextAccessor
                   .HttpContext
                   .Response
                   .Cookies
                   .Delete(cartName);

                httpContextAccessor
                   .HttpContext
                   .Response
                   .Cookies
                   .Append(
                        cartName,
                        json,
                        new CookieOptions()
                        {
                            Expires = DateTime.Now.AddDays(1)
                        });
            }
        }

        public CookieCartService(IProductService productService,
            IHttpContextAccessor httpContextAccessor)
        {
            this.productService = productService;
            this.httpContextAccessor = httpContextAccessor;

            cartName = "cart"
                        + (httpContextAccessor
                            .HttpContext.User.Identity.IsAuthenticated
                            ? httpContextAccessor.HttpContext.User.Identity.Name
                            : "");
        }


        public void AddToCart(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item != null)
                item.Quantity++;
            else
                cart.Items.Add(new CartItem { ProductId = id, Quantity = 1 });

            Cart = cart;
        }

        public void DecrementFromCart(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item is null)
                return;

            if (item.Quantity > 0)
                item.Quantity--;

            if (item.Quantity == 0)
                cart.Items.Remove(item);

            Cart = cart;
        }

        public void RemoveAll()
        {
            Cart.Items.Clear();
        }

        public void RemoveFromCart(int id)
        {
            var cart = Cart;

            var item = cart.Items.FirstOrDefault(x => x.ProductId == id);

            if (item is null)
                return;

            cart.Items.Remove(item);

            Cart = cart;
        }

        public CartViewModel TransformCart()
        {
            var products = productService.GetProducts(new ProductFilter()
            {
                Ids = Cart.Items.Select(i => i.ProductId).ToList()
            }).Select(p => new ProductViewModel()
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Order = p.Order,
                Price = p.Price,
                Brand = p.Brand != null ? p.Brand.Name : string.Empty
            }).ToList();

            var r = new CartViewModel
            {
                Items = Cart.Items.ToDictionary(
                    x => products.First(y => y.Id == x.ProductId),
                    x => x.Quantity)
            };

            return r;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Models;

namespace WebStoreGusev.Infrastructure.Services
{
    public class InMemoryCarService : ICarsService
    {
        List<CarViewModel> _cars;

        public InMemoryCarService()
        {
            _cars = new List<CarViewModel>
            {
                new CarViewModel
                {
                    Id = 100010,
                    Company = "Porshe",
                    Model = "911 Carrera",
                    Color = "Красный",
                    Price = 10_000_000
                },

                new CarViewModel
                {
                    Id = 200010,
                    Company = "BMW",
                    Model = "Z4 Roadster",
                    Color = "Синий",
                    Price = 4_500_000
                },

                new CarViewModel
                {
                    Id = 300010,
                    Company = "ВАЗ",
                    Model = "2101",
                    Color = "Бежевый",
                    Price = 10_000
                }
            };
        }

        public void AddNew(CarViewModel model)
        {
            model.Id = _cars.Max(e => e.Id) + 100000;
            _cars.Add(model);
        }

        public void Commit()
        {
            //throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            CarViewModel car = GetById(id);
            if (car is null)
                return;

            _cars.Remove(car);
        }

        public IEnumerable<CarViewModel> GetAll()
        {
            return _cars;
        }

        public CarViewModel GetById(int id)
        {
            return _cars.FirstOrDefault(x => x.Id == id);
        }
    }
}

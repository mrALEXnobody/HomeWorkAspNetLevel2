using Microsoft.AspNetCore.Mvc;
using WebStoreGusev.Infrastructure.Interfaces;
using WebStoreGusev.Models;

namespace WebStoreGusev.Controllers
{
    [Route("Cars")]
    public class CarController : Controller
    {
        private readonly ICarsService _carsService;

        public CarController(ICarsService carsService)
        {
            _carsService = carsService;
        }

        [Route("All")]
        public IActionResult Index()
        {
            return View(_carsService.GetAll());
        }

        [Route("{id}")]
        public IActionResult Buy(int id)
        {
            return View(_carsService.GetById(id));
        }

        public IActionResult Delete(int id)
        {
            _carsService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [Route("Edit/{Id?}")]
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            // add
            if (!id.HasValue)
                return View(new CarViewModel());

            // edit
            var model = _carsService.GetById(id.Value);

            if (model == null)
                return NotFound();

            return View(model);
        }

        [Route("Edit/{Id?}")]
        [HttpPost]
        public IActionResult Edit(CarViewModel carModel)
        {
            // edit 
            if (carModel.Id > 0)
            {
                var dbItem = _carsService.GetById(carModel.Id);

                if (ReferenceEquals(dbItem, null))
                    return NotFound();

                dbItem.Company = carModel.Company;
                dbItem.Model = carModel.Model;
                dbItem.Color = carModel.Color;
                dbItem.Price = carModel.Price;
            }
            else  // add
            {
                _carsService.AddNew(carModel);
            }

            // for Data Base
            _carsService.Commit();

            return RedirectToAction(nameof(Index));
        }

    }
}
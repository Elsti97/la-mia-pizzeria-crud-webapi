using Microsoft.AspNetCore.Mvc;
using la_mia_pizzeria_static.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace la_mia_pizzeria_static.Controllers
{
    [Authorize (Roles = "Admin,User")]
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;
        private readonly PizzeriaContext _context;

        public PizzaController(ILogger<PizzaController> logger, PizzeriaContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var pizzas = _context.Pizzas
                .Include(p => p.Category)
                .DefaultIfEmpty()
                .ToArray();

            return View(pizzas);

        }

        public IActionResult Detail(int id)
        {
            var pizza = _context.Pizzas.Include(p => p.Ingredients).Include(p => p.Category).DefaultIfEmpty().SingleOrDefault(p => p.Id == id);

            if (pizza is null)
            {
                return NotFound($"Pizza numero: {id} non trovata");
            }

            return View(pizza);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var pizzaFormModel = new PizzaFormModel
            {
                Categories = _context.Categories.ToList(),
                Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToList(),
            };

            return View(pizzaFormModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(PizzaFormModel pizzaFormModel)
        {
            if (!ModelState.IsValid)
            {
                pizzaFormModel.Categories = _context.Categories.ToList();
                pizzaFormModel.Ingredients = _context.Ingredients.Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToList();

                return View(pizzaFormModel);
            }

            pizzaFormModel.Pizza.Ingredients = pizzaFormModel.SelectedIngredients?.Select(si => _context.Ingredients.First(i => i.Id == Convert.ToInt32(si))).ToList();

            _context.Pizzas.Add(pizzaFormModel.Pizza);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var pizza = _context.Pizzas.Include(p => p.Ingredients).Include(p => p.Category).DefaultIfEmpty().SingleOrDefault(p => p.Id == id);

            if (pizza is null)
            {
                return View($"Pizza numero: {id} non trovata");
            }

            var pizzaFormModel = new PizzaFormModel
            {
                Pizza = pizza,
                Categories = _context.Categories.ToList(),
                Ingredients = _context.Ingredients.ToList().Select(i => new SelectListItem(i.Name, i.Id.ToString(), pizza.Ingredients!.Any(_i => _i.Id == i.Id))).ToList(),
            };

            pizzaFormModel.SelectedIngredients = pizzaFormModel.Ingredients.Where(i => i.Selected).Select(i => i.Value).ToList();

            return View(pizzaFormModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(int id, PizzaFormModel pizzaFormModel)
        {
            if (!ModelState.IsValid)
            {
                pizzaFormModel.Categories = _context.Categories.ToList();
                pizzaFormModel.Ingredients = _context.Ingredients.ToList().Select(i => new SelectListItem(i.Name, i.Id.ToString())).ToList();

                return View(pizzaFormModel);
            }

            var pizzaUpdate = _context.Pizzas.Include(p => p.Ingredients).FirstOrDefault(p => p.Id == id);

            if (pizzaUpdate is null)
            {
                return View($"Pizza numero: {id} non trovata");
            }

            pizzaUpdate.Nome = pizzaFormModel.Pizza.Nome;
            pizzaUpdate.Descrizione = pizzaFormModel.Pizza.Descrizione;
            pizzaUpdate.Prezzo = pizzaFormModel.Pizza.Prezzo;
            pizzaUpdate.CategoryId = pizzaFormModel.Pizza.CategoryId;
            pizzaUpdate.Ingredients = pizzaFormModel.SelectedIngredients?.Select(si => _context.Ingredients.First(i => i.Id == Convert.ToInt32(si))).ToList();

            _context.Pizzas.Update(pizzaUpdate);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var EliminaPizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (EliminaPizza is null)
            {
                return View("NotFound");
            }

            _context.Pizzas.Remove(EliminaPizza);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

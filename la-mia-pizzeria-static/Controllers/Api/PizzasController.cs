using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_static.Models;

namespace la_mia_pizzeria_static.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class PizzasController : ControllerBase
    {

        private readonly PizzeriaContext _context;

        public PizzasController(PizzeriaContext context)
        {
            _context = context;
        }


        [HttpGet]
        public IActionResult GetPizzas([FromQuery] string? nome)
        {
            var pizzas = _context.Pizzas.Where(p => nome == null || (p.Nome ?? "").Contains(nome)).ToList();
            return Ok(pizzas);
        }

        [HttpGet("{id}")]
        public IActionResult GetPizzaById(int id)
        {
            var pizzaSingola = _context.Pizzas.FirstOrDefault(p => p.Id == id);
            if (pizzaSingola == null)
            {
                return NotFound();
            }
            return Ok(pizzaSingola);
        }

        [HttpPost]
        public IActionResult CreatePizza([FromBody] Pizza pizza)
        {
            _context.Pizzas.Add(pizza);
            _context.SaveChanges();

            return Ok(pizza);
        }

        [HttpPut("{id}")]
        public IActionResult UpdatePizza(int id, [FromBody] Pizza pizza)
        {
            var updatePizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (updatePizza == null)
            {
                return NotFound();
            }

            updatePizza.Nome = pizza.Nome;
            updatePizza.Descrizione = pizza.Descrizione;
            updatePizza.Prezzo = pizza.Prezzo;
            updatePizza.CategoryId = pizza.CategoryId;

            _context.SaveChanges();

            return Ok(updatePizza);
        }

        [HttpDelete("{id}")]
        public IActionResult DeletePizza(int id)
        {
            var pizza = _context.Pizzas.FirstOrDefault(p => p.Id == id);

            if (pizza == null)
            {
                return NotFound();
            }

            _context.Pizzas.Remove(pizza);
            _context.SaveChanges();

            return Ok();
        }
    }
}

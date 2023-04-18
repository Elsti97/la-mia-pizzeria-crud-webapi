using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaFormModel
    {
        public Pizza Pizza { get; set; }
        public List<Category>? Categories { get; set; }

        public List<SelectListItem>? Ingredients { get; set; }

        [Required(ErrorMessage = "Selezionare almeno un ingrediente")]
        public List<string>? SelectedIngredients { get; set;}
    }
}

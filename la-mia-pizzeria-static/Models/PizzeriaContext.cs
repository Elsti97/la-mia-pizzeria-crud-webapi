using Microsoft.EntityFrameworkCore;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;



public class PizzeriaContext : IdentityDbContext<IdentityUser>
{

    public PizzeriaContext(DbContextOptions<PizzeriaContext> options) : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer("Data Source=localhost;Initial Catalog=db-pizzeria;Integrated Security=True;TrustServerCertificate=True;");
    }

    public DbSet<Pizza> Pizzas { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }

    public void Seed()
    {
        if (!Ingredients.Any())
        {
            var seedIngredients = new Ingredient[]
            {
            new Ingredient { Name = "Pomodoro" },
            new Ingredient { Name = "Mozzarella" },
            new Ingredient { Name = "Prosciutto crudo" },
            new Ingredient { Name = "Prosciutto cotto" },
            new Ingredient { Name = "Funghi" },
            new Ingredient { Name = "Salame" },
            new Ingredient { Name = "Rucola" },
            new Ingredient { Name = "Salsiccia" },
            new Ingredient { Name = "Wustel" },
            new Ingredient { Name = "Patatine" },
            };

            Ingredients.AddRange(seedIngredients);

            SaveChanges();
        }

        if (!Categories.Any())
        {
            var seedCategories = new Category[]
            {
            new Category { Name = "Classiche" },
            new Category { Name = "Bianche" },
            new Category { Name = "Vegetariane" },
            new Category { Name = "Di Mare" },
            };

            Categories.AddRange(seedCategories);

            SaveChanges();
        }

        if (!Pizzas.Any())
        {
            var seed = new Pizza[]
            {
                    new Pizza
                    {
                        Nome = "Margherita",
                        Descrizione = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Aenean rutrum magna quis lorem pellentesque, ut mattis odio interdum. Suspendisse vel bibendum eros, non ullamcorper odio",
                        Prezzo = 5.50M,
                        CategoryId = 1
                    },
                    new Pizza
                    {
                        Nome = "Diavola",
                        Descrizione = "Morbi dapibus, purus vel consequat pretium, orci ante aliquet urna, id pretium dolor orci et nunc. Nam imperdiet mi ligula, in lobortis magna iaculis ac",
                        Prezzo = 6.50M,
                        CategoryId = 2
                    }
            };

            Pizzas.AddRange(seed);

            SaveChanges();
        }

        if (!Roles.Any())
        {
            var seed = new IdentityRole[]
            {
                    new("Admin"),
                    new("User")
            };

            Roles.AddRange(seed);
        }

        if (Users.Any(u => u.Email == "pippo@gmail.com" || u.Email == "mario@gmail.com") && !UserRoles.Any())
        {
            var admin = Users.First(u => u.Email == "pippo@gmail.com");
            var user = Users.First(u => u.Email == "mario@gmail.com");

            var adminRole = Roles.First(r => r.Name == "Admin");
            var userRole = Roles.First(r => r.Name == "User");

            var seed = new IdentityUserRole<string>[]
            {
                    new()
                    {
                        UserId = admin.Id,
                        RoleId = adminRole.Id
                    },
                    new()
                    {
                        UserId = user.Id,
                        RoleId = userRole.Id
                    }
            };

            UserRoles.AddRange(seed);
        }

        SaveChanges();
    }
}

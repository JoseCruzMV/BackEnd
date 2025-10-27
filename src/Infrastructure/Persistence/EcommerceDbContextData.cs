using Ecommerce.Application.Models.Authorization;
using Ecommerce.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Ecommerce.Infrastructure.Persistence;

public class EcommerceDbContextData
{
    public static async Task LoadDataAsync(
        EcommerceDbContext context,
        UserManager<Usuario> userManager,
        RoleManager<IdentityRole> roleManager,
        ILoggerFactory loggerFactory)
    {
        try
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Role.ADMIN));
                await roleManager.CreateAsync(new IdentityRole(Role.USER));
            }
            if (!userManager.Users.Any())
            {
                var usuarioAdmin = new Usuario
                {
                    Nombre = "admin",
                    Apellido = "admin",
                    Email = "a@mail.com",
                    UserName = "admin.admin",
                    Telefono = "5512345678",
                    AvatarUrl = "https://firebasestorage.googleapis.com/v0/b/edificacion-app.appspot.com/o/vaxidrez.jpg?alt=media&token=14a28860-d149-461e-9c25-9774d7ac1b24"
                };

                await userManager.CreateAsync(usuarioAdmin, "PasswordAdmin123@");
                await userManager.AddToRoleAsync(usuarioAdmin, Role.ADMIN);

                var usuario = new Usuario
                {
                    Nombre = "user",
                    Apellido = "user",
                    Email = "user@mail.com",
                    UserName = "user.user",
                    Telefono = "5512345678",
                    AvatarUrl = "https://firebasestorage.googleapis.com/v0/b/edificacion-app.appspot.com/o/avatar-1.webp?alt=media&token=58da3007-ff21-494d-a85c-25ffa758ff6d"
                };

                await userManager.CreateAsync(usuario, "PasswordUser123@");
                await userManager.AddToRoleAsync(usuario, Role.USER);
            }

            if (!context.Categories.Any())
            {
                var categoryData = File.ReadAllText("../Infrastructure/Data/category.json");
                var categories = JsonConvert.DeserializeObject<List<Category>>(categoryData);

                await context.Categories.AddRangeAsync(categories!);
                await context.SaveChangesAsync();
            }

            if (!context.Products.Any())
            {
                var productData = File.ReadAllText("../Infrastructure/Data/product.json");
                var products = JsonConvert.DeserializeObject<List<Product>>(productData);

                await context.Products.AddRangeAsync(products!);
                await context.SaveChangesAsync();
            }

            if (!context.Images.Any())
            {
                var imagesData = File.ReadAllText("../Infrastructure/Data/image.json");
                var images = JsonConvert.DeserializeObject<List<Image>>(imagesData);

                await context.Images.AddRangeAsync(images!);
                await context.SaveChangesAsync();
            }

            if (!context.Reviews.Any())
            {
                var reviewsData = File.ReadAllText("../Infrastructure/Data/review.json");
                var reviews = JsonConvert.DeserializeObject<List<Review>>(reviewsData);

                await context.Reviews.AddRangeAsync(reviews!);
                await context.SaveChangesAsync();
            }

            if (!context.Countries.Any())
            {
                var countriesData = File.ReadAllText("../Infrastructure/Data/countries.json");
                var countries = JsonConvert.DeserializeObject<List<Country>>(countriesData);

                await context.Countries.AddRangeAsync(countries!);
                await context.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            var logger = loggerFactory.CreateLogger<EcommerceDbContextData>();
            logger.LogError(e.Message);
        }
    }
}
using Microsoft.AspNetCore.Identity;
using Store.Data.Entities.IdentityEntities;

namespace Store.Repository
{
    public class StoreIdentityContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName ="Wesam Morsy",
                    Email="wesamkhaledmorsy@gmail.com",
                    UserName="WesamKhaled",
                    Address= new Address
                    {
                        FirstName="Wesam",
                        LastName="Morsy",
                        City="Cairo",
                        State="Cairo",
                        Street="Kablat",
                        PostalCode="123466",
                    }
                };
                await userManager.CreateAsync(user,"Password123!");
            }
        }
    }
}

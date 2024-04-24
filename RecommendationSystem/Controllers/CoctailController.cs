using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecommendationSystem.Models;

public class CocktailController : Controller
{
    public async Task<IActionResult> Index()
    {
        using (var client = new HttpClient())
        {
            var response = await client.GetAsync("https://www.thecocktaildb.com/api/json/v1/1/search.php?s=margarita");
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                var cocktails = JsonConvert.DeserializeObject<List<Cocktail>>(content);

                return View(cocktails);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
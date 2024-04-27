using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecommendationSystem.Models;

namespace RecommendationSystem.Controllers;

public class CocktailController : Controller
{
    public async Task<IActionResult> Index()
    {
        var client = new HttpClient();
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://www.thecocktaildb.com/api/json/v1/1/search.php?s=margarita"),
        };


        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var root = JsonConvert.DeserializeObject<Root>(content);
        return View(root.drinks);
    }
    
}
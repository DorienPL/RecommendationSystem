using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RecommendationSystem.Helpers;
using RecommendationSystem.Models;

namespace RecommendationSystem.Controllers;
public class CocktailController : Controller
{ 
    public async Task<IActionResult> Random()
    {
        var client = new HttpClient();
        
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri("https://www.thecocktaildb.com/api/json/v1/1/random.php"),
        };


        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var random = JsonConvert.DeserializeObject<Root>(content);
        random.drinks[0].ingredients = IngredientHelper.ConvertIngredients(random.drinks[0]);
        return View(random.drinks);
    }
    [HttpGet]
    public IActionResult SearchByName()
    {
        return View();
    }
    [HttpPost]
    public async Task<IActionResult> Search(string cocktailName)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://www.thecocktaildb.com/api/json/v1/1/search.php?s={cocktailName}"),
        };

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var searchResult = JsonConvert.DeserializeObject<Root>(content);

        if (searchResult.drinks != null && searchResult.drinks.Count > 0)
        {
            foreach (var drink in searchResult.drinks)
            {
                drink.ingredients = IngredientHelper.ConvertIngredients(drink);
            }
            return View("Random", searchResult.drinks);
        }
        else
        {
            return View("NoResults");
        }
        
    }
    [HttpGet]
    public IActionResult SearchByIngredient()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SearchByIngredient(string ingredient)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"https://www.thecocktaildb.com/api/json/v1/1/filter.php?i={ingredient}"),
        };

        using var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        var searchResult = JsonConvert.DeserializeObject<Root>(content);

        if (searchResult.drinks != null && searchResult.drinks.Count > 0)
        {
            var fullDetails = new List<Drink>();
            foreach (var drink in searchResult.drinks)
            {
                var detailRequest = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri($"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={drink.idDrink}"),
                };
                using var detailResponse = await client.SendAsync(detailRequest);
                detailResponse.EnsureSuccessStatusCode();
                var detailContent = await detailResponse.Content.ReadAsStringAsync();
                var detailResult = JsonConvert.DeserializeObject<Root>(detailContent);

                if (detailResult.drinks != null && detailResult.drinks.Count > 0)
                {
                    var detailedDrink = detailResult.drinks[0];
                    detailedDrink.ingredients = IngredientHelper.ConvertIngredients(detailedDrink);
                    fullDetails.Add(detailedDrink);
                }
            }
            return View("Random", fullDetails);
        }
        else
        {
            return View("NoResults");
        }
    }
}
using System.Reflection;
using RecommendationSystem.Models;

namespace RecommendationSystem.Helpers;

public class IngredientHelper
{

    public static List<Ingredient> ConvertIngredients(Drink drinkDetails)
    {
        List<string> ingredientsList = new List<string>();
        List<string> measurementsList = new List<string>();
        List<Ingredient> result = new List<Ingredient>();
        Type recipeType = typeof(Drink);
        PropertyInfo[] properties = recipeType.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.Name.StartsWith("strIngredient") && property.PropertyType == typeof(string))
            {
                string ingredient = (string)property.GetValue(drinkDetails);
                if (!string.IsNullOrEmpty(ingredient))
                {
                    ingredientsList.Add(ingredient);
                }
            }
            if (property.Name.StartsWith("strMeasure") && property.PropertyType == typeof(string))
            {
                string measure = (string)property.GetValue(drinkDetails);
                if (!string.IsNullOrEmpty(measure))
                {
                    measurementsList.Add(measure);
                }
            }
        }

        for (int i = 0; i < ingredientsList.Count(); i++)
        {
            result.Add(new Ingredient()
            {
                name = ingredientsList[i],
                measure = measurementsList.Count() > i ? measurementsList[i] : "na oko"
            });
        }

        return result;
    }
}
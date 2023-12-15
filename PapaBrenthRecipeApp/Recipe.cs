using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st10090477_PROG6221_POE_GROUP_2
{
    // Class representing a recipe
    public class Recipe
    {
        // initializing delegate
        public delegate void ExceededCaloriesEventHandler(string message, double totalCalories);
        public event ExceededCaloriesEventHandler ExceededCalories;
        public string RecipeName { get; set; }
        public List<Ingredient> Ingredients = new List<Ingredient>();
        private List<double> OriginalQuantities = new List<double>();
        private List<string> OriginalUnits = new List<string>();
        private List<double> OriginalCalories = new List<double>();
        public List<string> Steps = new List<string>();



        public Recipe()
        {

        }


        // Constructor for creating a recipe
        public Recipe(string recipeName, List<Ingredient> ingredients, List<double> originalQuantities, List<string> originalUnits, List<string> steps)
        {
            RecipeName = recipeName;
            Ingredients = ingredients;
            OriginalQuantities = originalQuantities;
            OriginalUnits = originalUnits;
            Steps = steps;

            foreach (Ingredient ingredient in ingredients)
            {
                OriginalCalories.Add(ingredient.Calories);
            }
        }

        public string DisplayRecipe()
        {
            if (Ingredients.Count == 0)
            {
                return "No ingredients have been added.";
            }
            else
            {
                StringBuilder info = new StringBuilder($"Recipe Name: {RecipeName}\n");

                // Adding the description below the Recipe Name
                string description = GenerateDescription();
                info.AppendLine(description);

                info.AppendLine("Ingredients:");
                foreach (Ingredient ingredient in Ingredients.OrderBy(i => i.Name))
                {
                    info.AppendLine($"\t• {ingredient.Quantity} {ingredient.UnitOfMeasurement} - {ingredient.Name} (Food Group: {ingredient.FoodGroup})");
                }

                info.AppendLine($"\nSteps for making {RecipeName}:");
                for (int i = 0; i < Steps.Count; i++)
                {
                    info.AppendLine($"\t{i + 1}. {Steps[i]}");
                }

                info.AppendLine("\nAdditional Information:");
                return info.ToString();
            }
        }




        // Scale the recipe by a given factor
        public void ScaleRecipe(double scalingNumber)
        {
            for (int i = 0; i < Ingredients.Count; i++)
            {
                double quantity = Ingredients[i].Quantity;
                string ufm = Ingredients[i].UnitOfMeasurement;

                quantity *= scalingNumber;

                if (ufm == "tablespoon" || ufm == "tablespoons")
                {
                    if (quantity >= 16)
                    {
                        quantity /= 16;
                        quantity = Math.Round(quantity, 1);
                        ufm = "cup";
                    }
                }

                Ingredients[i].Quantity = quantity;
                Ingredients[i].UnitOfMeasurement = ufm;
                Ingredients[i].Calories *= scalingNumber;  // Scale the calories
            }
        }

        // Reset the recipe to its original values
        public bool ResetRecipe()
        {
            if (Ingredients.Count == 0)
            {
                return false;
            }
            else
            {
                for (int i = 0; i < OriginalQuantities.Count; i++)
                {
                    Ingredients[i].Quantity = OriginalQuantities[i];
                    Ingredients[i].UnitOfMeasurement = OriginalUnits[i];
                    Ingredients[i].Calories = OriginalCalories[i];  // Reset calories
                }
                return true;
            }
        }

        public double TotalCalories()
        {
            // Initialize a variable to store the total calories.
            double totalCalories = 0;

            // Iterate through each ingredient in the Ingredients list.
            foreach (Ingredient ingredient in Ingredients)
            {
                // Add the calories of the current ingredient to the total.
                totalCalories += ingredient.Calories;
            }

            // Return the total calories.
            return totalCalories;
        }

        // Notify the event handler if the total calories exceed a certain threshold
        public void notifyer()
        {
            double totalCalories = TotalCalories();
            if (totalCalories > 300)
            {
                ExceededCalories?.Invoke($"\t* Alert!! Total calories for {RecipeName} exceeded 300! This is a high-calorie recipe. You might want to consider a lighter option.", totalCalories);
            }
            else if (totalCalories > 200)
            {
                ExceededCalories?.Invoke($"\t* Total calories for {RecipeName} are between 200 and 300. This is a moderately high-calorie recipe. Please keep in mind your daily intake.", totalCalories);
            }
            else if (totalCalories > 100)
            {
                ExceededCalories?.Invoke($"\t* Total calories for {RecipeName} are between 100 and 200. This is a moderate-calorie recipe. It's a balanced choice.", totalCalories);
            }
            else
            {
                ExceededCalories?.Invoke($"\t* Total calories for {RecipeName} are less than 100. This is a low-calorie recipe. Great choice for weight management.", totalCalories);
            }
        }

        // Add this GenerateDescription method inside the Recipe class
        public string GenerateDescription()
        {
            StringBuilder description = new StringBuilder("This recipe for ");
            description.Append(RecipeName);
            description.Append(" uses ");

            for (int i = 0; i < Ingredients.Count; i++)
            {
                Ingredient ingredient = Ingredients[i];
                description.Append(ingredient.Quantity);
                description.Append(" ");
                description.Append(ingredient.UnitOfMeasurement);
                description.Append(" of ");
                description.Append(ingredient.Name);

                if (i < Ingredients.Count - 2)
                {
                    description.Append(", ");
                }
                else if (i == Ingredients.Count - 2)
                {
                    description.Append(" and ");
                }
            }

            description.Append(" to create a delightful preparation.");
            return description.ToString();
        }


    }
}

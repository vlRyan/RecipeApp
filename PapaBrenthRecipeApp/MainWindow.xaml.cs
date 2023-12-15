using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace st10090477_PROG6221_POE_GROUP_2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Ingredient> allIngredients;
        private List<double> allOriginalQuantities;
        private List<string> allOriginalUnits;
        private List<string> allSteps;
        private string recipeName;
        Recipe recipe;
        private static Dictionary<string, Recipe> Recipes = new Dictionary<string, Recipe>();

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += Window_Loaded;

        }

        private void btnAddRecipe_Click(object sender, RoutedEventArgs e)
        {
            AddIngredient addIngredient = new AddIngredient();
            addIngredient.ShowDialog();
            // Retrieve the ingredient details from the AddIngredient window

            allIngredients = addIngredient.ingredients;
            allOriginalQuantities = addIngredient.originalQuantities;
            allOriginalUnits = addIngredient.originalUnits;
            allSteps = addIngredient.steps;
            recipeName = addIngredient.recipeName;
            if (recipeName != null)
            {
                recipe = new Recipe(recipeName, allIngredients, allOriginalQuantities, allOriginalUnits, allSteps);

                // Check if the recipe name already exists in the dictionary

                if (!Recipes.ContainsKey(recipe.RecipeName))
                {
                    // Add the recipe to the dictionary
                    Recipes.Add(recipe.RecipeName, recipe);
                    lstDisplayDetails.Items.Clear();
                    LoadRecipeNames(); // Call the method that loads recipe names
                }
                else
                {
                    MessageBox.Show("A recipe with this name already exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        // Event handler for the "Display All Recipes" button
        private void btnDisplayAllRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (Recipes.Count == 0)
            {
                displayNoRecipe();
            }
            else
            {
                lstDisplayDetails.Items.Clear();
                List<string> sortedKeys = new List<string>(Recipes.Keys);
                sortedKeys.Sort();

                foreach (string key in sortedKeys)
                {
                    Recipe recipe = Recipes[key];

                    // Unsubscribe from the ExceededCalories event to prevent multiple subscriptions
                    recipe.ExceededCalories -= ExceededCaloriesHandler;

                    // Subscribe to the ExceededCalories event
                    recipe.ExceededCalories += ExceededCaloriesHandler;
                    lstDisplayDetails.Items.Add(recipe.DisplayRecipe());
                    recipe.notifyer(); // Call Notifyer method which internally calculates total calories
                    lstDisplayDetails.Items.Add("-------------------------------------------------------" +
                        "-------------------------------------------------------------------------------");
                }
            }
        }
        // Event handler for the "Display Specific Recipe" button
        private void btnDisplaySpecificRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (Recipes.Count == 0)
            {
                displayNoRecipe();
            }
            else
            {
                lstDisplayDetails.Items.Clear();
                string recipeName = txtSpecificRecipe.Text;
                if (Recipes.ContainsKey(recipeName))
                {
                    // Unsubscribe from the ExceededCalories event to prevent multiple subscriptions
                    recipe.ExceededCalories -= ExceededCaloriesHandler;

                    // Subscribe to the ExceededCalories event
                    recipe.ExceededCalories += ExceededCaloriesHandler;

                    lstDisplayDetails.Items.Add(Recipes[recipeName].DisplayRecipe());
                    double totalCalories = Recipes[recipeName].TotalCalories();
                    Recipes[recipeName].notifyer();

                   
                }
                else
                {
                    MessageBox.Show("Invalid recipe name please check the spelling of the recipe.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
        // Event handler for the ExceededCalories event
        private void ExceededCaloriesHandler(string message, double totalCalories)
        {
            // Concatenate total calories to the message
            string messageWithCalories = $"\t* Total calories (Calories quantify food energy intake): {totalCalories}\n" + message + "\n";

            // Add the message with total calories to the lstDisplayDetails ListBox
            lstDisplayDetails.Items.Add(messageWithCalories);
        }
       

        private void btnClearRecipe_Click(object sender, RoutedEventArgs e)
        {
            if (Recipes.Count == 0)
            {
                displayNoRecipe();
            }
            else
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to clear all recipes? (y/n):", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
                lstDisplayDetails.Items.Clear();

                if (result == MessageBoxResult.Yes)
                {
                    Recipes.Clear();
                    txtSpecificRecipe.Items.Clear();
                    MessageBox.Show("All recipes have been cleared.", "Success", MessageBoxButton.OK);
                }
                else
                {
                    MessageBox.Show("You cancled to clear the recipe.", "Cancled", MessageBoxButton.OK);
                }
            }

        }
        
        // Method to display a message when there are no recipes
        private void displayNoRecipe()
        {
            MessageBox.Show("There is no recipe in the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
       
      
        // Event handler when the window is loaded
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var recipeName in Recipes.Keys)
            {
                txtSpecificRecipe.Items.Add(recipeName);
            }

        }
        // Method to load recipe names into the text boxes
        private void LoadRecipeNames()
        {
            txtSpecificRecipe.Items.Clear();
            foreach (var recipeName in Recipes.Keys)
            {
                txtSpecificRecipe.Items.Add(recipeName);
            }
        }
    }
}

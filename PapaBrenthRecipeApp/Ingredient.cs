using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace st10090477_PROG6221_POE_GROUP_2
{
    public class Ingredient
    {
        public string Name { get; set; } // The name of the ingredient
        public double Quantity { get; set; } // The quantity of the ingredient
        public string UnitOfMeasurement { get; set; } // The unit of measurement for the quantity

        private double _calories; // The calorie content of the ingredient
        public double Calories
        {
            get { return _calories; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("Calories cannot be negative.");
                }
                _calories = value;
            }
        }

        public string FoodGroup { get; set; } // The food group to which the ingredient belongs

    }
}

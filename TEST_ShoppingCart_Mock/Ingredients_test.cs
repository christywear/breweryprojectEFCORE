using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using BreweryEFCoreClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace TEST_ShoppingCart_Mock
{
    [TestFixture]
    public class Ingredients_Test
    {
        private bitsContext dbContext;
        private List<Ingredient> ingList;
        private List<RecipeIngredient> recipeIngList;
        private List<Recipe> recipeList;
        private List<IngredientType> ingTypeList;
        private List<IngredientInventoryAddition> ingOrderList;
        private List<Supplier> supList;

        [SetUp]
        public void Setup()
        {
            dbContext = new bitsContext();
            recipeList = dbContext.Recipe.Where(s => s.RecipeId == 1).ToList();
            recipeIngList = dbContext.RecipeIngredient.Where(s => s.RecipeId == 1).ToList();
            ingList = dbContext.Ingredient.OrderBy(s => s.IngredientId).ToList();
            ingTypeList = dbContext.IngredientType.OrderBy(s => s.IngredientTypeId).ToList();
            ingOrderList = dbContext.IngredientInventoryAddition.OrderBy(s => s.IngredientInventoryAdditionId).ToList();
            supList = dbContext.Supplier.OrderBy(s => s.SupplierId).ToList();

        }

        [Test]
        public void GetAllTest()
        {

           
            // print list of needed recipe ingredients based off recipe id 1
            Console.WriteLine("Ingredients for recipe " + recipeList[0].Name + ": ");
            PrintAll(recipeIngList);


        }

        public void PrintAll(List<RecipeIngredient> inv)
        {
            foreach (RecipeIngredient I in inv)
            {
                Console.Write("Name of Ingredient:  " + ingList[I.IngredientId - 1].Name);
                Console.Write("  , Type:  " + ingTypeList[ingList[I.IngredientId - 1].IngredientTypeId - 1].Name);
                Console.Write("  , Amount on Order:  " + ingOrderList[I.RecipeIngredientId - 1].Quantity);
                Console.Write("  , Amount in inventory:  " + ingList[I.IngredientId - 1].OnHandQuantity);
                Console.Write("  , Scheduled to use:  " + recipeIngList[I.RecipeIngredientId - 1].Quantity);
                Console.Write("  , Cost per unit:  " + ingList[I.IngredientId - 1].UnitCost);
                Console.Write("  , SubTotal:  " + (ingList[I.IngredientId - 1].UnitCost * (decimal)ingOrderList[ingOrderList.FindIndex(x => x.IngredientId == I.IngredientId)].Quantity));
                Console.Write("  , Supplier : " + supList[ingOrderList[ingOrderList.FindIndex(x => x.IngredientId == I.IngredientId)].SupplierId - 1].Name);
                Console.WriteLine();


            }

        }

    }

}
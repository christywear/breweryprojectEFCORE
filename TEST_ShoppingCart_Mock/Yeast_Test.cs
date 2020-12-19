using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using BreweryEFCoreClasses.Models;
using Microsoft.EntityFrameworkCore;
namespace TEST_ShoppingCart_Mock
{
    [TestFixture]
    public class Yeast_Test
    {
        private bitsContext dbContext;
        Yeast Y;
        Ingredient I;
        List<Yeast> yeasts;

        [SetUp]
        public void Setup()
        {
            dbContext = new bitsContext();
        }

        [Test]
        public void GetAllTest()
        {
            yeasts = dbContext.Yeast.OrderBy(Y => Y.IngredientId).ToList();
            Assert.AreEqual(483, yeasts.Count);
            Assert.AreEqual("WLP530", yeasts[1].ProductId);
            PrintAll(yeasts);
        }

        [Test]
        public void GetByPrimaryKeyTest()
        {
            Y = dbContext.Yeast.Find(272);
            Assert.IsNotNull(Y);
            Assert.AreEqual("White Labs", Y.Laboratory);
            Console.WriteLine(Y);
        }
        
        [Test]
        public void GetUsingWhere()
        {
            // get a list of all of the yeast which has a low flocculation
            yeasts = dbContext.Yeast.Where(y => y.Flocculation == ("Low")).OrderBy(Y => Y.ProductId).ToList();
            Assert.AreEqual(115, yeasts.Count);
            Assert.AreEqual(271, yeasts[0].IngredientId);
            PrintAll(yeasts);

        }
             
        
        [Test]
        public void GetWithJoinTest()
        {
            // get a list of objects that include the name of yeast, and contact info for labs that make it
            var yeasts = dbContext.Yeast.Join(
               dbContext.Supplier,
               Y => Y.Laboratory,
               s => s.Name,
               (Y, s) => new {Y.ProductId, s.Name, s.Phone, s.ContactPhone, s.ContactFirstName, s.ContactLastName, s.ContactEmail, s.Note }).OrderBy(r => r.Name).ToList();
            Assert.AreEqual(103, yeasts.Count);
            foreach( var i in yeasts)
            {
                Console.WriteLine(i + "\n");
            }
        }
        
        [Test]
        public void CreateTest()
        {
            Y = new Yeast();
            Y.ProductId = "EE101RED";
            Y.MinTemp = 2.0;
            Y.MaxTemp = 44.22;
            Y.Form = "Non-Newtonian";
            Y.Laboratory = "S.T.A.R. Labs";
            Y.Flocculation = "I'm not giggling you are..";
            Y.Attenuation = 1.21;
            Y.MaxReuse = Int32.MaxValue;
            Y.Type = "Flux Capacitance";
            Y.BestFor = "Time travel";
            I = new Ingredient();
            I.Name = "The Time Traveler";
            I.IngredientTypeId = 4;
            I.Version = 1;
            I.OnHandQuantity = 0;
            I.UnitCost = 0.00m;
            I.UnitTypeId = 2;
            I.Notes = "A swirling mass of wibbly wobbly colourful time stuff";
            dbContext.Ingredient.Add(I);
            dbContext.SaveChanges();
            Y.IngredientId = I.IngredientId;
            dbContext.Yeast.Add(Y);
            dbContext.SaveChanges();
            List<Ingredient> myIngredient = dbContext.Ingredient.Where(I => I.Name == "The Time Traveler").ToList();
            Assert.IsNotNull(dbContext.Ingredient.Find(I.IngredientId));
            List<Yeast> myyeast = dbContext.Yeast.Where(Y => Y.ProductId == "EE101RED").ToList();
            Assert.IsNotNull(dbContext.Yeast.Find(Y.IngredientId));
        }
                
        [Test]
        public void UpdateTest()
        {
            Y = dbContext.Yeast.Find(272);
            if (Y.ProductId == "WLP530")
            {
                Y.ProductId = "If this works, I'll see you yesterday";
            }
            else
            {
                Y.ProductId = "WLP530";
            }
            
            dbContext.Yeast.Update(Y);
            dbContext.SaveChanges();
            Y = dbContext.Yeast.Find(272);
            Assert.That(
                "WLP530" == dbContext.Yeast.Find(272).ProductId ||
                "If this works, I'll see you yesterday" ==
                dbContext.Yeast.Find(272).ProductId);
            
        }
        
        [Test]
        public void DeleteTest()
        {
            var yeasts = from yeet in dbContext.Yeast
                        where yeet.ProductId == "EE101RED"
                        select yeet;

            List<Ingredient> myIngredients = new List<Ingredient>();
            myIngredients = dbContext.Ingredient.Where(x => x.Name == "The Time Traveler").ToList();
            myIngredients.Reverse();
            //var myyeast = dbContext.Yeast.SingleOrDefault(Yeast => Yeast.IngredientId == 1175);
            //var myIngredient = dbContext.Ingredient.SingleOrDefault(I => I.IngredientId == 1175);
            List<Ingredient> check_ing = new List<Ingredient>();
            List<Yeast> check_yeast = new List<Yeast>();
            foreach (var I in myIngredients)
            {
                if(I.IngredientTypeId == 4)
                {
                    Yeast grr = new Yeast(); 
                    grr = yeasts.FirstOrDefault(x => x.IngredientId == I.IngredientId);
                    dbContext.Yeast.Remove(grr);
                    
                }
                dbContext.Ingredient.Remove(I);
                
                check_ing.Add(I);

            }

            dbContext.SaveChanges();

            foreach (Ingredient I in check_ing)
            {
                Assert.IsNull(dbContext.Ingredient.Find(I.IngredientId));
            }


            foreach (Yeast Y in yeasts)
            {
                dbContext.Yeast.Remove(Y);
                check_yeast.Add(Y);
            }

            dbContext.SaveChanges();

            foreach (Yeast Y in check_yeast)
            {
                Assert.IsNull(dbContext.Yeast.Find(Y.IngredientId));
            }
            
        }
       
        public void PrintAll(List<Yeast> inv)
        {
            foreach (Yeast I in inv)
            {
                Console.WriteLine(I);
            }
        }

    }
}

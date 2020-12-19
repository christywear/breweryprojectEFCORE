using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using BreweryEFCoreClasses.Models;
using Microsoft.EntityFrameworkCore;

namespace TEST_ShoppingCart_Mock
{
    [TestFixture]
    public class Supplier_Test
    {
        private bitsContext dbContext;
        private Supplier sup;
        private SupplierAddress sA;
        private AddressType addrT;
        private Address addr;
       
        private List<Supplier> supList;
        private List<SupplierAddress> supAddrList;
        private List<AddressType> addTypeList;
        private List<Address> addresses;

        [SetUp]
        public void Setup()
        {
            dbContext = new bitsContext();
            supList = dbContext.Supplier.OrderBy(s => s.SupplierId).ToList();
            supAddrList = dbContext.SupplierAddress.OrderBy(s => s.SupplierId).ToList();
            addTypeList = dbContext.AddressType.OrderBy(s => s.AddressTypeId).ToList();
            addresses = dbContext.Address.OrderBy(s => s.AddressId).ToList();

        }

        [Test]
        public void GetAllTest()
        {
            
            sup = supList[0]; // get supplier 
            int supid = sup.SupplierId; // get supplier id
            sA = supAddrList[supid]; // get supplier addr id+type using sup id info
            int addtypeid = sA.AddressTypeId; // get specific addr type id
            addrT = addTypeList[addtypeid]; // get specific addr type  row with id
            string addTypeString = addrT.Name; // get name of type of addr.
            int addrID = sA.AddressId; // get addr id
            addr = addresses[addrID];
            string address_Street = addr.StreetLine1 + " " + addr.StreetLine2;
            string address_city = addr.City;
            string address_state = addr.State;
            string address_zip = addr.Zipcode;
            string address_country = addr.Country;
            Assert.AreEqual(6, supList.Count);
            Assert.AreEqual("Malteurop Malting Company", supList[supid].Name);

            //testing output
            //company name
            Console.WriteLine("Company Name:\n" + supList[supid].Name + "\n");
            
            //company phone
            Console.WriteLine("Company Phone Number: \n" + supList[supid].Phone + "\n");
            
            //company addr
            Console.WriteLine(addTypeString + " Address:\n" + address_Street);
            Console.WriteLine(address_city + ", " + address_state + " " +
                              address_state + " " + address_zip);
            Console.WriteLine(address_country + "\n");

            //Contact info
            //account db empty atm

            // notes
            Console.WriteLine("Notes: " + supList[0].Note + "\n");

        }


    }

}
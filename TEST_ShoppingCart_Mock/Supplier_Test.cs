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
        }

        [Test]
        public void GetAllTest()
        {
            supList = dbContext.Supplier.OrderBy(s => s.SupplierId).ToList();
            Assert.AreEqual(6, supList.Count);
            Assert.AreEqual("Malteurop Malting Company", supList[1].Name);
            PrintAll(supList);
        }

        public void PrintAll(List<Supplier> printList)
        {
            foreach (var I in printList)
            {
                Console.WriteLine(I.Name);
            }
        }
    }

}

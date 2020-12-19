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

        [Test]
        public void SetCompany()
        {
            Address newTemp = new Address();
            Supplier tempSupplier = new Supplier();
            SupplierAddress supdataspec = new SupplierAddress();

            //create supplier
            tempSupplier.Name = "Neato Labs";
            tempSupplier.Phone = "541-555-1122";
            tempSupplier.Note = "Very neat.";
            tempSupplier.ContactFirstName = "Christy";
            tempSupplier.ContactLastName = "Wear";
            tempSupplier.ContactPhone = "541-555-1123";
            
            
            //create address
            newTemp.StreetLine1 = "123 neato st.";
            newTemp.StreetLine2 = "Suite 1";
            newTemp.City = "Eugene";
            newTemp.State = "Oregon";
            newTemp.Country = "USA";
            

            //add primary data (supplier, and address)
            if(!supList.Exists(x => x.Name == tempSupplier.Name))
            {
                dbContext.Supplier.Add(tempSupplier);
            }
            if (!addresses.Exists(x => x.StreetLine1 == newTemp.StreetLine1))
            {              
                dbContext.Address.Add(newTemp);
            }
            dbContext.SaveChanges();

            //update supplier adddress table
            sup = dbContext.Supplier.SingleOrDefault(x => x.Name == "Neato Labs");
            addr = dbContext.Address.SingleOrDefault(x => x.StreetLine1 == "123 neato st.");
            SupplierAddress supMapAddr = new SupplierAddress();
            supMapAddr.AddressId = addr.AddressId;
            supMapAddr.SupplierId = sup.SupplierId;
            supMapAddr.AddressTypeId = addTypeList[addTypeList.FindIndex(x => x.Name == "mailing")].AddressTypeId; // mailing
            dbContext.SupplierAddress.Add(supMapAddr);
            dbContext.SaveChanges();
            supMapAddr = dbContext.SupplierAddress.SingleOrDefault(x => x.SupplierId == sup.SupplierId);

            //verify
            Assert.IsNotNull(supMapAddr);

            //Verify and output
            sup = dbContext.Supplier.SingleOrDefault(x => x.Name == tempSupplier.Name);
            addr = dbContext.Address.SingleOrDefault(x => x.StreetLine1 == newTemp.StreetLine1);

            Assert.IsNotNull(sup);
            Assert.IsNotNull(addr);
            Assert.AreEqual("Neato Labs", sup.Name);
            Assert.AreEqual("123 neato st.",addr.StreetLine1);

            PrintInfo(addr, sup);
        }

        [Test]
        public void UpdateCompany()
        {
            //get info

            if (addresses.FindIndex(x => x.StreetLine1 == "123 neato st.") == -1) 
            {
                if (addresses.FindIndex(x => x.StreetLine1 == "1010 farmvalley rd.") != -1) 
                {
                    //woops been ran before need to reset it
                    sup = dbContext.Supplier.SingleOrDefault(x => x.Name == "Neato Labs");
                    addr = dbContext.Address.SingleOrDefault(x => x.StreetLine1 == "1010 farmvalley rd.");
                    addr.StreetLine1 = "123 neato st.";
                    sup.Email = "";
                    dbContext.Address.Update(addr);
                    dbContext.Supplier.Update(sup);
                    dbContext.SaveChanges();
                }
            }
            sup = dbContext.Supplier.SingleOrDefault(x => x.Name == "Neato Labs");
            addr = dbContext.Address.SingleOrDefault(x => x.StreetLine1 == "123 neato st.");


            //lets update.
            addr.StreetLine1 = "1010 farmvalley rd.";
            addr.StreetLine2 = "";
            dbContext.Address.Update(addr);

            sup.ContactEmail = "realemail@gmail.com";
            dbContext.Supplier.Update(sup);
            dbContext.SaveChanges();

            // verify
            Assert.IsNotNull(sup);
            Assert.IsNotNull(addr);
            sup = dbContext.Supplier.SingleOrDefault(x => x.Name == "Neato Labs");
            addr = dbContext.Address.SingleOrDefault(x => x.StreetLine1 == "1010 farmvalley rd.");

            Assert.AreEqual("realemail@gmail.com", sup.ContactEmail);
            Assert.AreEqual("1010 farmvalley rd.", addr.StreetLine1);

            
            
            //output
            PrintInfo(addr, sup);

        }

        [Test]
        public void DeleteSupplier()
        {
            SupplierAddress supAddr = new SupplierAddress();
            //update links to data
            if (supList.FindIndex(x => x.Name == "Neato Labs") == -1)
                sup = null;
            else
                sup = sup = dbContext.Supplier.SingleOrDefault(x => x.Name == "Neato Labs");
            if (sup == null || supAddrList.FindIndex(x => x.SupplierId == sup.SupplierId) == -1)
                supAddr = null;
            else
            {
                supAddr = dbContext.SupplierAddress.SingleOrDefault(x => x.SupplierId == sup.SupplierId);
            }
            if (addresses.FindIndex(x => x.StreetLine1 == "123 neato st.") == -1)
            {
                if (addresses.FindIndex(x => x.StreetLine1 == "1010 farmvalley rd.") != -1)
                {
                    addr = dbContext.Address.SingleOrDefault(x => x.StreetLine1 == "1010 farmvalley rd.");

                }
            }
            else if (addresses.FindIndex(x => x.StreetLine1 == "123 neato st.") == -1)
            {
                addr = null;
            }

            //verify
            if (sup == null)
            {
                Assert.Fail("Address is null");
            }
            if (supAddr == null)
            {
                Assert.Fail("address map is not found");
            }
            if (addr == null)
            {
                Assert.Fail("supplier is not found");
            }

            // remove
            dbContext.SupplierAddress.Remove(supAddr);
            supAddrList.Remove(supAddr);
            supAddr = null;
            dbContext.Supplier.Remove(sup);
            supList.Remove(sup);
            sup = null;
            dbContext.Address.Remove(addr);
            addresses.Remove(addr);
            addr = null;
            dbContext.SaveChanges();

            //update links to data
            if (supList.FindIndex(x => x.Name == "Neato Labs") == -1)
                sup = null;
            else
                sup = sup = dbContext.Supplier.SingleOrDefault(x => x.Name == "Neato Labs");
            if (sup == null || supAddrList.FindIndex(x => x.SupplierId == sup.SupplierId) == -1)
                supAddr = null;
            else
            {
                supAddr = dbContext.SupplierAddress.SingleOrDefault(x => x.SupplierId == sup.SupplierId);
            }
            if (addresses.FindIndex(x => x.StreetLine1 == "123 neato st.") == -1)
            {
                if (addresses.FindIndex(x => x.StreetLine1 == "1010 farmvalley rd.") != -1)
                {
                    addr = dbContext.Address.SingleOrDefault(x => x.StreetLine1 == "1010 farmvalley rd.");

                }
            }
            else if (addresses.FindIndex(x => x.StreetLine1 == "123 neato st.") == -1)
            {
                addr = null;
            }

            // verify deletion

            if(sup != null)
            {
                Assert.Fail("sup is not deleted");
            }
            if(supAddr != null)
            {
                Assert.Fail("supplier address type map is not deleted");
            }
            if(addr != null)
            {
                Assert.Fail("Supplier Address has not been deleted");
            }


        }

        public void PrintInfo(Address addr, Supplier sup)
        {
            Console.WriteLine("Supplier reads as :");
            Console.WriteLine("ID :  " + sup.SupplierId);
            Console.WriteLine("Name :  " + sup.Name);
            Console.WriteLine("Phone :  " + sup.Phone);
            Console.WriteLine("Email :  " + sup.Email);
            Console.WriteLine("Contact First Name :  " + sup.ContactFirstName);
            Console.WriteLine("Contact Last Name :  " + sup.ContactLastName);
            Console.WriteLine("Contact Phone :  " + sup.ContactPhone);
            Console.WriteLine("Contact Email :  " + sup.ContactEmail);

            Console.WriteLine("\n\n Supplier address follows  :");
            Console.WriteLine("ID  :" + addr.AddressId);
            Console.WriteLine("Street Line 1  :" + addr.StreetLine1);
            Console.WriteLine("Street Line 2  :" + addr.StreetLine2);
            Console.WriteLine("City  :" + addr.City);
            Console.WriteLine("State  :" + addr.State);
            Console.WriteLine("Zip  :" + addr.Zipcode);
            Console.WriteLine("Country  :" + addr.Country);
        }
    }

}
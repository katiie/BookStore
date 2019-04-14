using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookstore.Core.Models;
using Bookstore.Data.Model;
using Bookstore.Service.IReposervices;
using Bookstore.Service.Reposervices;
using System.Transactions;
using FizzWare.NBuilder;
using System.Collections.Generic;
using System.Linq;

namespace BookStoreUnitTest
{
    [TestClass]
    public class StoreServiceTest
    {
        private string Address;
        private string State;
        IStoreService _StoreService;
       
        [TestInitialize]
        public void SetUp()
        {
            _StoreService = new StoreService();
            InitialiseParameters();
        }

        private void InitialiseParameters()
        {
            Address = Faker.Address.StreetAddress();
            State = Faker.Address.City();

        }


        [TestMethod]
        public void StoreCrud()
        {
            Guid newID = Create();
            CreateWithoutAllRequiredField_returnFalse();
            GetByID(newID);
            GetAll();
            Update(newID);
            Delete(newID);
        }



        /// <summary>
        /// Creates this instance.
        /// </summary>
        /// <returns>The id of the new record.</returns>
        private Guid Create()
        {
            // Arrange
            Store Store = Builder<Store>.CreateNew().With(s => s.Id = Guid.NewGuid())
                .With(s => s.Address = Address)
                .With(s => s.State = Address)
                .With(s => s.Longitude = Faker.RandomNumber.Next())
                 .With(s => s.Latitude = Faker.RandomNumber.Next())
                .Build();

            // Act
            var storeId = _StoreService.Add(Store);

            // Assert
            Assert.AreNotEqual(Guid.Empty, Store.Id, "Creating new record does not return id");

            var sId= _StoreService.Add(Store);
            Assert.AreEqual(Guid.Empty, sId, "Creating new record does not return id");


            return storeId;
        }

        private void CreateWithoutAllRequiredField_returnFalse()
        { 
            // Arrange
            Store Store = Builder<Store>.CreateNew().With(s => s.Id = Guid.NewGuid())
                .With(s => s.Address = Address)
                .With(s => s.State = Address)
                .Build();

            // Act
            var id = _StoreService.Add(Store);

            // Assert
            Assert.AreEqual(Guid.Empty, id, "Creating new record does not return id");

        }
        
        private void CreateWithoutAllRequiredField()
        {

            // Arrange
            Store Store = Builder<Store>.CreateNew().With(s => s.Id = Guid.NewGuid())
                .With(s => s.Address = Faker.Address.StreetAddress())
                .With(s => s.State = Faker.Address.City())
                .Build();


            // Act
            _StoreService.Add(Store);

            // Assert
            Assert.AreEqual(Guid.Empty, Store.Id, "Address has been added id");


        }


        private void Update(Guid Id)
        {
            // Arrange
            State = Faker.Address.City() + "New"; ;
            Store Store = _StoreService.Get(Id);
            Store.State = State;

            // Act
            _StoreService.Edit(Store);

            Store updatedStore = _StoreService.Get(Id);

            // Assert
            Assert.AreEqual(State, updatedStore.State, "Record is  updated.");
            Assert.AreSame(Store, updatedStore, "Record is  updated.");


        }


        /// <summary>
        /// Gets all.
        /// </summary>
        private void GetAll()
        {
            // Act
            IEnumerable<Store> items = _StoreService.Gets();

            // Assert
            Assert.IsTrue(items.ToList().Count() > 0, "GetAll returned no items.");
        }

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="id">The id of the Store.</param>
        private void GetByID(Guid ID)
        {
            // Act
            Store Store = _StoreService.Get(ID);

            // Assert
            Assert.IsNotNull(Store, "GetByID returned null.");
            Assert.AreEqual(ID, Store.Id);

        }



        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        private void Delete(Guid Id)
        {
            // Arrange
            Store Store = _StoreService.Get(Id);

            // Act
            _StoreService.Delete(Id);
            Store = _StoreService.Get(Id);

            // Assert
            Assert.IsNull(Store, "Record is deleted.");
        }
    }
}

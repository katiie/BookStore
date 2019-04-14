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
    public class StockServiceTest
    {
         private IStockService _StockService;
        private IBookService _BookService;
        private StoreService _StoreService;
        private Store Store;
        private Book Book;
        private DateTime CreatedOn;
        private string Email;
        private DateTime ModifiedOn;
        private string FullName;


        /// <summary>
        /// Sets up.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _BookService = new BookService();
            _StoreService = new StoreService();
            _StockService = new StockService();
            InitialiseParameters();
        }

        private void InitialiseParameters()
        {
            Store = _StoreService.Gets().FirstOrDefault();
            Book = _BookService.Gets().FirstOrDefault();
        }

       
        [TestMethod]
        public void StockCrud()
        {
            Guid newID = Create();
           // MakeRequest(newID);
            GetByID(newID);
            GetAll();
            Update(newID);
            Delete(newID);
        }

       
        /// Creates this instance.
        /// </summary>
        /// <returns>The id of the new record.</returns>
        private Guid Create()
        {
            // Arrange
            Stock Stock = Builder<Stock>.CreateNew().With(s => s.Id = Guid.NewGuid())
                .With(s => s.BookId = Book.Id)
                .With(s => s.StoreId = Store.Id)
                 .With(s => s.Quantity = Faker.RandomNumber.Next(2))
                .Build();

            // Act
            var StockId = _StockService.Add(Stock);

            // Assert
            Assert.AreNotEqual(Guid.Empty, Stock.Id, "Creating new record returns id");

            Stock = Builder<Stock>.CreateNew().With(s => s.Id = Guid.NewGuid())
                .With(s => s.BookId = Book.Id)
                .With(s => s.StoreId = Store.Id)
                .Build();


            Stock.Id = _StockService.Add(Stock);
            Assert.AreNotEqual(Guid.Empty, Stock.Id, "Creating new record does not return id");


            return StockId;
        }

     
        private void Update(Guid Id)
        {
            // Arrange
            Stock Stock = _StockService.Get(Id);
            Stock.Quantity = Faker.RandomNumber.Next(2);

            // Act
            _StockService.Edit(Stock);

            Stock updatedStock = _StockService.Get(Id);

            // Assert
            Assert.AreEqual(Stock.Quantity, updatedStock.Quantity, "Record is not updated.");

        }


        /// <summary>
        /// Gets all.
        /// </summary>
        private void GetAll()
        {
            // Act
            IEnumerable<Stock> items = _StockService.Gets();

            // Assert
            Assert.IsTrue(items.ToList().Count() > 0, "GetAll returned no items.");
        }

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="id">The id of the User.</param>
        private void GetByID(Guid ID)
        {
            // Act
            Stock Stock = _StockService.Get(ID);

            // Assert
            Assert.IsNotNull(Stock, "GetByID returned null.");
            Assert.AreEqual(ID, Stock.Id);
           
        }

       
        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        private void Delete(Guid Id)
        {
            // Arrange
            Stock Stock = _StockService.Get(Id);

            // Act
            _StockService.Delete(Id);
            Stock = _StockService.Get(Id);

            // Assert
            Assert.IsNull(Stock, "Record is not deleted.");
        }
    }
}

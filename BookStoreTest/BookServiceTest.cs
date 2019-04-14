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
    //store should have been added
    [TestClass]
    public class BookServiceTest
    {
        IBookService _BookService;
        IStoreService _storeService;

        private Store Store;
       
        /// <summary>
        /// Sets up.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _BookService = new BookService();
            _storeService = new StoreService();
            InitialiseParameters();
        }

        private void InitialiseParameters()
        {
            Store = _storeService.Gets().FirstOrDefault();
        }

       
        [TestMethod]
        public void BookCrud()
        {
            Guid newID = Create();
            GetByID(newID);
            GetAll();
            Update(newID);
            Delete(newID);
        }

      
        private Guid Create()
        {
            // Arrange
            Book Book = Builder<Book>.CreateNew().With(s => s.Id = Guid.NewGuid())
                .With(s => s.Author = Faker.Name.FullName())
                .With(s => s.Description = Faker.Lorem.Paragraph())
                .With(s => s.ISBN = Faker.Identification.UKNationalInsuranceNumber())
                 .With(s => s.Name = Faker.Name.First())
                .Build();

            // Act
            var BookId = _BookService.Add(Book);

            // Assert
            Assert.AreNotEqual(Guid.Empty, BookId, "Creating new record does not return id");

            Book = Builder<Book>.CreateNew().With(s => s.Id = Guid.Empty)
                .With(s => s.ISBN = Faker.Identification.UKNationalInsuranceNumber())
                 .With(s => s.Name = Faker.Name.First())
                .Build();


            var sId = _BookService.Add(Book);
            Assert.AreEqual(sId,Book.Id,  "Creating new record does not return id");


            return BookId;
        }
       
        private void Update(Guid Id)
        {
            // Arrange
            Book Book = _BookService.Get(Id);
            Book.Name = Faker.Name.First();

            // Act
            _BookService.Edit(Book);

            Book updatedBook = _BookService.Get(Id);

            // Assert
            Assert.AreEqual(Book.Name, updatedBook.Name, "Record is not updated.");

        }


        /// <summary>
        /// Gets all.
        /// </summary>
        private void GetAll()
        {
            // Act
            IEnumerable<Book> items = _BookService.Gets();

            // Assert
            Assert.IsTrue(items.ToList().Count() > 0, "GetAll returned no items.");
        }

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="id">The id of the Book.</param>
        private void GetByID(Guid ID)
        {
            // Act
            Book Book = _BookService.Get(ID);

            // Assert
            Assert.IsNotNull(Book, "GetByID returned null.");
            Assert.AreEqual(ID, Book.Id);
           
        }

       
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        private void Delete(Guid Id)
        {
            // Arrange
            Book Book = _BookService.Get(Id);

            // Act
            _BookService.Delete(Id);
            Book = _BookService.Get(Id);

            // Assert
            Assert.IsNull(Book, "Record is not deleted.");
        }
    }
}

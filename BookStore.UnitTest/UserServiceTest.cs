using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Bookstore.Core.Models;
using Moq;
using Bookstore.Data.Model;
using Bookstore.Service.IReposervices;
using Bookstore.Service.Reposervices;
using System.Transactions;
using FizzWare.NBuilder;
using System.Collections.Generic;
using System.Linq;

namespace BookStore.UnitTest
{
    [TestClass]
    public class UserServiceTest
    {
        IUserService _userService;

        private DateTime CreatedOn;
        private string Email;
        private DateTime ModifiedOn;
        private string FullName;
        private Guid id;


        /// <summary>
        /// Sets up.
        /// </summary>
        [TestInitialize]
        public void SetUp()
        {
            _userService = new UserService();
            InitialiseParameters();
        }

        private void InitialiseParameters()
        {
            CreatedOn = DateTime.Now;
            Email = "test@test.com";
            id = new Guid("05D5FD46-263E-E211-BFBA-1040F3A7A3B1");
            ModifiedOn = DateTime.Now;
            FullName = "Who is Luke Skywalkers father?";
           
        }

       
        [TestMethod]
        public void UserCrud()
        {
            Guid newID = Create();
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
            User User = new User();
            User.DateCreated = DateTime.Now;
            User.DateModified = DateTime.Now;
            User.Id = Guid.NewGuid();
            User.Email = Email;
            User.FullName = FullName;

            // Act
            _userService.AddUser(User);

            // Assert
            Assert.AreNotEqual(Guid.Empty, User.Id, "Creating new record does not return id");

            return User.Id;
        }
        [TestMethod]
        private void Update(Guid Id)
        {
            // Arrange
            User User = _userService.GetUser(Id);
            User.FullName = "Seun ade";

            // Act
            _userService.EditUser(User);

            User updatedUser = _userService.GetUser(id);

            // Assert
            Assert.AreNotEqual(FullName, updatedUser.FullName, "Record is not updated.");

        }


        /// <summary>
        /// Gets all.
        /// </summary>
        private void GetAll()
        {
            // Act
            IEnumerable<User> items = _userService.GetAllUser();

            // Assert
            Assert.IsTrue(items.ToList().Count() > 0, "GetAll returned no items.");
        }

        /// <summary>
        /// Gets the by ID.
        /// </summary>
        /// <param name="id">The id of the User.</param>
        private void GetByID(Guid id)
        {
            // Act
            User User = _userService.GetUser(id);

            // Assert
            Assert.IsNotNull(User, "GetByID returned null.");
            Assert.AreEqual(id, User.Id);
            Assert.AreEqual(Email, User.Email);
           
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        private void Delete(Guid id)
        {
            // Arrange
            User User = _userService.GetUser(id);

            // Act
            _userService.RemoveUser(id);
            User = _userService.GetUser(id);

            // Assert
            Assert.IsNull(User, "Record is not deleted.");
        }
    }
}

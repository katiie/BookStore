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
    public class UserServiceTest
    {
        IUserService _userService;

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
            _userService = new UserService();
            InitialiseParameters();
        }

        private void InitialiseParameters()
        {
            CreatedOn = DateTime.Now;
            Email = "test@bookstore.cms";
            ModifiedOn = DateTime.Now;
            FullName = "Who is Luke Skywalkers father?";
           
        }

       
        [TestMethod]
        public void UserCrud()
        {
            Guid newID = Create();
            GetByID(newID);
            GetAll();
            GetByEmailReturnValue(Email);
            GetByEmailReturnNull(Email);
            CreateBadEmail();
            CreateWithExisitingEmailReturnEmpty();
            Update(newID);
            Delete(newID);
        }

        private void GetByEmailReturnNull(string email)
        {
            User User = _userService.GetByEmail("one@one.com");

            // Assert
            Assert.IsNull(User, "Email present returned null.");
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

        private void CreateBadEmail()
        {
            // Arrange
            User User = new User();
            User.DateCreated = DateTime.Now;
            User.DateModified = DateTime.Now;
            User.Id = Guid.NewGuid();
            User.Email = "temi";
            User.FullName = FullName;

            // Act
            var result =_userService.AddUser(User);

            // Assert
            Assert.AreEqual(Guid.Empty, result, "Email not in the right format");

        }
        private void CreateWithExisitingEmailReturnEmpty()
        {
            // Arrange
            User User = new User();
            User.DateCreated = DateTime.Now;
            User.DateModified = DateTime.Now;
            User.Id = Guid.NewGuid();
            User.Email = Email;
            User.FullName = FullName;

            // Act
           var result= _userService.AddUser(User);

            // Assert
            Assert.AreEqual(Guid.Empty, result, "Email exist");

        }

        private void Update(Guid Id)
        {
            // Arrange
            User User = _userService.GetUser(Id);
            User.FullName = "Seun ade";

            // Act
            _userService.EditUser(User);

            User updatedUser = _userService.GetUser(Id);

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
        private void GetByID(Guid ID)
        {
            // Act
            User User = _userService.GetUser(ID);

            // Assert
            Assert.IsNotNull(User, "GetByID returned null.");
            Assert.AreEqual(ID, User.Id);
            Assert.AreEqual(Email, User.Email);
           
        }

        private void GetByEmailReturnValue(string Email)
        {
            // Act
            User User = _userService.GetByEmail(Email);

            // Assert
            Assert.IsNotNull(User, "Email present returned null.");
           
        }

        /// <summary>
        /// Deletes the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        private void Delete(Guid Id)
        {
            // Arrange
            User User = _userService.GetUser(Id);

            // Act
            _userService.RemoveUser(Id);
            User = _userService.GetUser(Id);

            // Assert
            Assert.IsNull(User, "Record is not deleted.");
        }
    }
}

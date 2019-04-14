using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookstore.Core.Models;
using Bookstore.Data.UnitOfWork;
using Bookstore.Service.IReposervices;
using System.Net.Mail;

namespace Bookstore.Service.Reposervices
{
    public class UserService : IUserService
    {
        private UnitOfWork uow = new UnitOfWork();

        public Guid AddUser(User User)
        {
            try
            {
                if (GetByEmail(User.Email) != null || !IsValid( User.Email))
                    return Guid.Empty;
                uow.RepositoryFor<User>().Add(User);
                uow.Save();
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
            return User.Id;
        }

        public Guid AddOrEditUser(User User)
        {
            if (User.Id == Guid.Empty)
                return AddUser(User);
            else
                if (EditUser(User))
                return User.Id;
            else
                return Guid.Empty;
        }

        public bool EditUser(User User)
        {
            try
            {
                uow.RepositoryFor<User>().Edit(User);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public IEnumerable<User> GetAllUser()
        {
            var List = uow.RepositoryFor<User>().GetAll();

            return uow.RepositoryFor<User>().GetAll();
        }

        public IEnumerable<User> GetAllUserIncludingDeleted()
        {
            return uow.RepositoryFor<User>().GetAll();
        }

        public User GetUser(Guid id)
        {
            return uow.RepositoryFor<User>().Get(id);
        }

        public int GetCounts()
        {
            return GetAllUser().Count();
        }

        public User GetByEmail(string email)
        {
            return uow.RepositoryFor<User>().Find(d => d.Email.ToLower() == email.ToLower()).FirstOrDefault();
        }


        public bool RemoveUser(Guid id)
        {
            try
            {
                var _getUser = GetUser(id);
                uow.RepositoryFor<User>().Remove(_getUser);
                uow.Save();
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
        public bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

    }
}

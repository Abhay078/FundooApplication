using CommonLayer.Model;
using ManagerLayer.Interface;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Services
{
    public class UserManager:IUserManager
    {
        private readonly IUserRepository repository;
        public UserManager(IUserRepository repository)
        {
            this.repository = repository;
        }
        public UserEntity UserRegister(UserRegModel model)
        {
            return repository.UserRegister(model);
        }
        public UserEntity Login(LoginModel model)
        {
            return repository.Login(model);
        }
        public string Generate(string Email, long Userid)
        {
            return repository.Generate(Email, Userid);
        }
        public IEnumerable<UserEntity> GetAllUser()
        {
            return repository.GetAllUser();
        }

        public UserEntity GetUserById(long id)
        {
            return repository.GetUserById(id);
        }

        public string ForgetPassword(string email)
        {
            return repository.ForgetPassword(email);
        }
        public bool ResetPassword(ResetPasswordModel ResetModel,string email)
        {
            return repository.ResetPassword(ResetModel,email);
        }
    }
}

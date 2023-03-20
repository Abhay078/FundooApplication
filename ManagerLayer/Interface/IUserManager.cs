using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ManagerLayer.Interface
{
    public interface IUserManager
    {
        public UserEntity UserRegister(UserRegModel model);
        public UserEntity Login(LoginModel model);
        public string Generate(string Email, long Userid);

       

        public string ForgetPassword(string email);
        public bool ResetPassword(ResetPasswordModel ResetModel,string email);
    }
}

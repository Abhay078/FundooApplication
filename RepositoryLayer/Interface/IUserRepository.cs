using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface IUserRepository
    {
        public UserEntity UserRegister(UserRegModel model);
        public UserEntity Login(LoginModel model);
        public string Generate(string Email, long Userid);

        public IEnumerable<UserEntity> GetAllUser();

        public UserEntity GetUserById(long id);

        public string ForgetPassword(string email);

        public bool ResetPassword(ResetPasswordModel ResetModel,string email);
    }
}

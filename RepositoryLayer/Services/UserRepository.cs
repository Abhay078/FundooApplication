using CommonLayer.Model;
using FundooNotesApplication;

using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using RepositoryLayer.Interface;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Services
{
    public class UserRepository : IUserRepository
    {

        private readonly FunContext context;
        private readonly IConfiguration _config;

        public UserRepository(FunContext context, IConfiguration config)
        {
            this.context = context;
            _config = config;
        }

        public UserEntity UserRegister(UserRegModel model)
        {
            try
            {
                UserEntity entity = new UserEntity();
                entity.FirstName = model.FirstName;
                entity.LastName = model.LastName;
                entity.Email = model.Email;
                entity.Password = EncryptPassword(model.Password);
                var Check = context.User.Add(entity);
                context.SaveChanges();

                if (Check != null)
                {

                    return entity;
                }
                else
                {

                    throw new CustomException("Database connection not stablished correctly");


                }

            }
            catch (CustomException)
            {


                throw;

            }

        }
        public string EncryptPassword(string password)
        {
            try
            {
                var PlainText = Encoding.UTF8.GetBytes(password);
                var EncodedPass = Convert.ToBase64String(PlainText);
                return EncodedPass;

            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public UserEntity Login(LoginModel model)
        {
            try
            {

                var CheckEmail = context.User.FirstOrDefault(v => v.Email == model.Email);
                if (CheckEmail != null)
                {
                    var CheckPass = context.User.FirstOrDefault(v => v.Password == EncryptPassword(model.Password));
                    if (CheckPass != null)
                    {

                        return CheckEmail;
                    }
                    else
                    {
                        throw new CustomException("user not found");


                    }
                }
                return null;

            }
            catch (Exception)
            {

                throw;
            }





        }
        public string Generate(string Email, long Userid)
        {
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                var claims = new[]
                {
                new Claim("Id",Userid.ToString()),
                new Claim(ClaimTypes.Email, Email),



            };

                var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                  _config["Jwt:Audience"],
                  claims,
                  expires: DateTime.Now.AddMinutes(15),
                  signingCredentials: credentials);

                return new JwtSecurityTokenHandler().WriteToken(token);

            }
            catch (Exception)
            {

                throw;
            }

        }



        public string ForgetPassword(string email)
        {
            try
            {
                var CheckEmail = context.User.Where(x => x.Email == email).FirstOrDefault();
                if (CheckEmail != null)
                {
                    var token = Generate(CheckEmail.Email, CheckEmail.UserId);
                    MSMQ msmq = new MSMQ();
                    msmq.sendData2Queue(token);

                    return token;
                }

                throw new CustomException("Email Not registered with us.. Retry with Registered one");


            }
            catch (Exception)
            {

                throw;

            }

        }

        public bool ResetPassword(ResetPasswordModel ResetModel, string email)
        {
            try
            {

                if (ResetModel.NewPassword.Equals(ResetModel.ConfirmPassword))
                {

                    var Check = context.User.FirstOrDefault(x => x.Email == email);
                    if (Check != null)
                    {
                        Check.Password = EncryptPassword(ResetModel.NewPassword);
                        context.SaveChanges();

                        return true;
                    }

                    throw new CustomException("User Not Found");



                }
                throw new CustomException("Password Not Matches");
            }
            catch (CustomException)
            {

                throw;

            }

        }
    }


}

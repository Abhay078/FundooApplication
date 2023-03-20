using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Entity;
using RepositoryLayer.FundooDBContext;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IConfiguration _config;
        private readonly IUserManager manager;
        public UserController(IConfiguration config, IUserManager manager)
        {
            _config = config;
            this.manager = manager;
        }
        [HttpPost]
        [Route("Register")]
        public ActionResult UserRegister(UserRegModel model)
        {
            try
            {
                var CheckReg = manager.UserRegister(model);
                if (CheckReg != null)
                {
                    return Ok(new ResponseModel<UserEntity> { Status = true, Message = "Register Successfull", Data = CheckReg });
                }
                else
                {
                    return BadRequest(new ResponseModel<UserEntity> { Status = false, Message = "Register unsuccessfull", Data = CheckReg });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }

        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public ActionResult UserLogin([FromBody] LoginModel model)
        {
            try
            {

                var checkLogin = manager.Login(model);
                if (checkLogin != null)
                {
                    var tokenString = manager.Generate(checkLogin.Email,checkLogin.UserId);
                    return Ok(new ResponseModel<string> { Status = true, Message="Login Successful",Data = tokenString });

                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Login Unsuccessfull" });
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;

            }


        }
        [HttpGet]
        public ActionResult GetAllUser ()
        {
            try
            {
                var CheckResult = manager.GetAllUser();
                if (CheckResult != null)
                {
                    return Ok(CheckResult);

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            return BadRequest(BadRequest(new ResponseModel<string> { Status = false, Message = "Failed to get data" }));

        }
        [HttpGet("{id}")]
        public ActionResult GetUserById(long id)
        {
            try
            {
                var CheckResult= manager.GetUserById(id);
                if (CheckResult != null)
                {
                    return Ok(CheckResult);
                }

            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return BadRequest(new ResponseModel<string> { Status = false, Message = "User Not Found" });
        }
        [HttpPost("ForgetPassword")]
        public ActionResult ForgetPassword(string email)
        {
            var CheckEmail=manager.ForgetPassword(email);
            if (CheckEmail != null)
            {
                return Ok(new ResponseModel<string> { Status=true,Message="Reset Link Send Successfully"});
            }
            return BadRequest(new ResponseModel<string> { Status = false, Message = "Reset Link Not send"});
        }
        [Authorize]
        [HttpPost("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel ResetModel)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var Check = manager.ResetPassword(ResetModel,email);
                if (Check)
                {
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Reset Password Successful",Data=Check});
                }
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Reset Unsuccessful", Data = Check });

            }
            catch (Exception)
            {

                throw;
            }
            
        }



    }
}

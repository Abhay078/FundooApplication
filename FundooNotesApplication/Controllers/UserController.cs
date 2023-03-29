using CommonLayer.Model;
using ManagerLayer.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RepositoryLayer.Entity;
using System;
using System.Security.Claims;

namespace FundooNotesApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        
        private readonly IUserManager manager;
        private readonly ILogger<UserController> logger;
        public UserController(IUserManager manager,ILogger<UserController> logger)
        {
            
            this.manager = manager;
            this.logger = logger;
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
                logger.LogError(ex.Message);
                throw;

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
                    var tokenString = manager.Generate(checkLogin.Email, checkLogin.UserId);
                    return Ok(new ResponseModel<string> { Status = true, Message = "Login Successful", Data = tokenString });

                }
                else
                {
                    return BadRequest(new ResponseModel<string> { Status = false, Message = "Login Unsuccessfull" });
                }

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;

            }


        }

        [HttpPost("ForgetPassword")]
        public ActionResult ForgetPassword(string email)
        {
            try
            {
                var CheckEmail = manager.ForgetPassword(email);
                if (CheckEmail != null)
                {
                    return Ok(new ResponseModel<string> { Status = true, Message = "Reset Link Send Successfully" });
                }
                return BadRequest(new ResponseModel<string> { Status = false, Message = "Reset Link Not send" });

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }
            
        }
        [Authorize]
        [HttpPost("ResetPassword")]
        public ActionResult ResetPassword(ResetPasswordModel ResetModel)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var Check = manager.ResetPassword(ResetModel, email);
                if (Check)
                {
                    return Ok(new ResponseModel<bool> { Status = true, Message = "Reset Password Successful", Data = Check });
                }
                return BadRequest(new ResponseModel<bool> { Status = false, Message = "Reset Unsuccessful", Data = Check });

            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                throw;
            }

        }



    }
}

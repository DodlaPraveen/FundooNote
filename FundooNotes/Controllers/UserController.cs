using BisinessLayer.Interfaces;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Linq;
using System.Security.Claims;

namespace FundooNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserBL userBL;

        //private static Logger logger = LogManager.GetCurrentClassLogger();

        private readonly ILogger<UserController> logger;
        public UserController(IUserBL userBL, ILogger<UserController> logger )
        {
            this.userBL = userBL;
            this.logger = logger;

        }
        [HttpPost("Register")]
        public IActionResult AddUser(UserRegistration userRegistration)
        {
            try
            {
                var reg = this.userBL.Registration(userRegistration);
                if (reg != null)

                {
                    logger.LogInformation("Registration is Sucess");
                    return this.Ok(new { Success = true, message = "Registration Sucessfull", Response = reg });
                }
                else
                {
                    logger.LogInformation("Registration is Sucess");
                    return this.BadRequest(new { Success = false, message = "Registration Unsucessfull" });
                }
            }
            catch (Exception ex)
            {
                return this.BadRequest(new { Success = false, message = ex.Message });
            }
        }
        //[Authorize]
        [HttpPost("Login")]
        public IActionResult Login(UserLogin userLogin)
        {
            try
            {
                string Login = userBL.Login(userLogin);
                if (Login != null)
                {
                    logger.LogInformation("Registration is Sucess");
                    return this.Ok(new { success = true, message = $"login successful for {userLogin.Email}", data = Login });
                }
                else
                {
                    logger.LogError("This is an error message");
                    return this.BadRequest(new { Success = false, message = $"login failed for {userLogin.Email}" });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost("Forgetpassword")]
        public IActionResult Forgetpassword(string email)
        {
            try
            {
                string token = userBL.Forgetpassword(email);
                if (token != null)
                {
                    return this.Ok(new { success = true, message = "please check your mail token send sucessfully}"});
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Email is not registered" });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        [Authorize]
        [HttpPost("Reset")]
        public IActionResult ResetPassword(string password, string confirmPassword)
        {
            try
            {
                var email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                //var email = User.Claims.First(e => e.Type == "Email").Value;
                if (userBL.ResetPassword(email, password, confirmPassword))
                {
                    return this.Ok(new { Success = true, message = "Your password has been changed sucessfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unable to reset password.Please try again" });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }

    
}

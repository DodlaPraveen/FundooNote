using CommonLayer.Model;
using ReposatoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace ReposatoryLayer.Interfaces
{
    public interface IUserRL
    {
        public UserEntity Registration(UserRegistration user);
        public string Login(UserLogin userlogin);
        public string GenerateJWTToken(long userid, string email);
        public string Forgetpassword(string email);
        public bool ResetPassword(string email, string password, string confirmpassword);
    }
  
}

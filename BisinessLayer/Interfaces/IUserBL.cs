using CommonLayer.Model;
using ReposatoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLayer.Interfaces
{
    public interface IUserBL
    {
        public UserEntity Registration(UserRegistration user);
        public string Login(UserLogin userlogin);
        public string GenerateJWTToken(long userid, string email);
        public string Forgetpassword(string email);
        public bool ResetPassword(string email, string password, string confirmpassword);
    }
}

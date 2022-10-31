using BisinessLayer.Interfaces;
using CommonLayer.Model;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interfaces;
using ReposatoryLayer.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace BisinessLayer.Services
{
    public class UserBL : IUserBL
    {
        IUserRL iuserrl;

        public UserBL(IUserRL iuserrl)
        {
            this.iuserrl = iuserrl;
        }

        public UserEntity Registration(UserRegistration user)
        {
            try
            {
                return this.iuserrl.Registration(user);
            }
            catch (Exception)
            {

                throw;
            }
        }
        public string Login(UserLogin userLogin)
        {
            try
            {
                return iuserrl.Login(userLogin);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public string GenerateJWTToken(long userid, string email)
        {
            try
            {
                return iuserrl.GenerateJWTToken(userid, email);
            }
            catch (Exception)
            {
                throw;
            }

        }
        public string Forgetpassword(string email)
        {
            try
            {
                return iuserrl.Forgetpassword(email);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public bool ResetPassword(string email, string password, string confirmpassword)
        {
            try
            {
                return iuserrl.ResetPassword(email, password, confirmpassword);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

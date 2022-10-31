using CommonLayer.Model;
using Experimental.System.Messaging;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ReposatoryLayer.AppControl;
using ReposatoryLayer.Entity;
using ReposatoryLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Claims;
using System.Text;

namespace ReposatoryLayer.Services
{
    public class UserRL : IUserRL

    {
        public readonly IConfiguration Iconfiguration;
        public readonly Context context;
        public UserRL(Context context, IConfiguration Iconfiguration)

        {
            this.context = context;
            this.Iconfiguration = Iconfiguration;
        }
        public UserEntity Registration(UserRegistration user)
        {
            try
            {
                UserEntity entity = new UserEntity();
                entity.FirstName = user.FirstName;
                entity.LastName = user.LastName;
                entity.Email = user.Email;
                entity.DateOfBirth = user.DateOfBirth;

                entity.Password = EncryptPass(user.Password);

                context.Users.Add(entity);

                int result = this.context.SaveChanges();
                if (result > 0)
                {
                    return entity;

                }
                return null;

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
                UserEntity user = new UserEntity();
                user = this.context.Users.FirstOrDefault(x => x.Email == userLogin.Email);
                string dPass = Decrpt(user.Password);
                var id = user.UserId;
                var email = user.Email;
                if (user != null && dPass == userLogin.Password)
                {
                    var token = GenerateJWTToken(id, email);
                    return token;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public string EncryptPass(string password)
        {
            try
            {
                string msg = "";
                byte[] encode = new byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                msg = Convert.ToBase64String(encode);
                return msg;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public string Decrpt(string encodedData)
        {
            try
            {
                System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
                System.Text.Decoder utf8Decode = encoder.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encodedData);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                string result = new String(decoded_char);
                return result;
            }
            catch (Exception)
            {

                throw;
            }
        }




        public string GenerateJWTToken(long userid, string email)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Iconfiguration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("UserId", userid.ToString()),
                    new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public string Forgetpassword(string email)
        {
            try
            {
                var result = this.context.Users.FirstOrDefault(x => x.Email == email);
                if (result != null)
                {
                    var token = this.GenerateJWTToken(result.UserId, email);
                    new MSMQ().sendData2Queue(token);
                    return token;
                }
                return null;
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
                if (password.Equals(confirmpassword))
                {
                    UserEntity user = context.Users.Where(e => e.Email == email).FirstOrDefault();
                    user.Password = confirmpassword;
                    context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}



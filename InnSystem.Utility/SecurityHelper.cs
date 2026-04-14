using System;
using System.Collections.Generic;
using System.Text;
using BCrypt.Net;

namespace InnSystem.Utility
{
    public class SecurityHelper
    {

        //Creacion de usuario
        public static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }


        //Inicio de sesion
        public static bool VerifyPassword(string password, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
        }

    }
}

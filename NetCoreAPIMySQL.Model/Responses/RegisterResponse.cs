using System;
using System.Collections.Generic;
using System.Text;

namespace NetCoreAPIMySQL.Model.Responses
{
    public class RegisterResponse
    {
        public string response{ get; set; }
        public int status { get; set; }

        public RegisterResponse(string message,int statusCode)
        {
            status = statusCode;
            response = message;
        }
    }
}

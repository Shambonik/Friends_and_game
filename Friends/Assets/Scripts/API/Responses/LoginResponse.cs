using System;
using System.Collections.Generic;
using System.Text;

namespace AureoleCore.Responses
{
    public class LoginResponse
    {
        public string error { get; set; }
        public LoginResponseData result { get; set; }
    }

    public class LoginResponseData
    {
        public string SessionId { get; set; }
    }

}
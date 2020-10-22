using AureoleCore.Models;

namespace AureoleCore.Responses
{
    public class RegisterResponse
    {
        public string error { get; set; }
        public User result { get; set; }
    }
}

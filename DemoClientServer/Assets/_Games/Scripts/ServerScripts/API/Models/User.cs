using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AureoleCore.Models
{
    public class User
    {
        public long userId { get; set; }

        public string androidId { get; set; }
        public string email { get; set; }

        public string nickname { get; set; }
    }
}
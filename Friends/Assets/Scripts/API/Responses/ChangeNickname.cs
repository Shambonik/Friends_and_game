using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AureoleCore.Responses
{
    public class ChangeNickname
    {
        public string session_id { get; set; }
        public string nickname { get; set; }
    }
}


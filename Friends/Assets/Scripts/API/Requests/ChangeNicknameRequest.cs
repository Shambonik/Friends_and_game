using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AureoleCore.Requests
{
    public class ChangeNicknameRequest
    {
        public string session_id { get; set; }
        public string nickname { get; set; }
    }
}
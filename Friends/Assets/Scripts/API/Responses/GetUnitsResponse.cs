using System.Collections;
using System.Collections.Generic;
using AureoleCore.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace AureoleCore.Responses
{
    public class GetUnitsResponse
    {
        public string error { get; set; }
        public List<UnitResponseData> result { get; set; }
    }
}

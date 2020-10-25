using System.Collections;
using System.Collections.Generic;
using AureoleCore.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace AureoleCore.Responses
{
    public class GetUnitsResponse
    {
        public Error error { get; set; }

        public UnitListResponseData data { get; set; }

        public class UnitListResponseData
        {
            public List<UnitListResponseModel> units { get; set; }
        }

        public class UnitListResponseModel
        {
            public long id { get; set; }
            public string name { get; set; }
            public string class_name { get; set; }

            public float hp { get; set; }
            public float mana { get; set; }

            public int level { get; set; }
            public int skill_points { get; set; }
        }
    }
}

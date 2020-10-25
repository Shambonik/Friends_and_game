using System.Collections.Generic;
using AureoleCore.Models;
using AureoleCore.Responses;
using AureoleCore.Utils;

namespace AureoleCore.UnitAPI
{
    /// <summary>
    /// API для работы с юнитами.
    /// </summary>
    public class UnitAPI
    {
        public static List<Race> GetAllRaces()
        {
            var races =  Net.Get<List<Race>>($"Unit/races");
            return races;
        }

        public static CreateNewCharacter CreateNewUnit(string androidId,  string _name,  string _class_name)
        {
            var data = Net.Post<CreateNewCharacter>($"user/unit/create", new {android_id = androidId, name = _name, class_name = _class_name});
            return data;
        }
        
        public static GetUnitsResponse GetUserUnits(string androidId)
        {
            GetUnitsResponse data = Net.Post<GetUnitsResponse>($"user/unit/list", new {android_id = androidId});
            return data;
        }
    }
}

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

        public static CreateNewCharacter CreateNewUnit(string sessionId,  string raceCode,  string unitName)
        {
            var data = Net.Post<CreateNewCharacter>($"Unit/create", new {session_id = sessionId, race_code = raceCode, unit_name = unitName});
            return data;
        }
    }
}

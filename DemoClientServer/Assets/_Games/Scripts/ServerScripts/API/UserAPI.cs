using AureoleCore.Utils;
using AureoleCore.Responses;

namespace AureoleCore.UserAPI
{
    /// <summary>
    /// API для работы с аккаунтом игрока.
    /// </summary>
    public class UserAPI
    {
        public static RegisterResponse CreateUser(string androidId)
        {
            RegisterResponse data = Net.Get<RegisterResponse>($"User/reg/{androidId}");
            return data;
        }

        public static LoginResponse LoginUser(string androidId)
        {
            LoginResponse data = Net.Get<LoginResponse>($"User/login/{androidId}");
            return data;
        }

        public static void ChangeNickname(string sessionId, string nickname)
        {
            Net.Post<ChangeNickname>($"User/change", new { session_id = sessionId, nickname = nickname});
        }

        public static GetUnitsResponse GetUserUnits(string sessionId)
        {
            GetUnitsResponse races = Net.Get<GetUnitsResponse>($"User/units/{sessionId}");
            return races;
        }
    }
}
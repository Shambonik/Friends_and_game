using System.Collections.Generic;
using AureoleCore.Utils;
using AureoleCore.Responses;
using UnityEngine;

namespace AureoleCore.UserAPI
{
    /// <summary>
    /// API для работы с аккаунтом игрока.
    /// </summary>
    public class UserAPI
    {

        public static CheckResponse CheckRegistered(string androidId)
        {
            CheckResponse data = Net.Post<CheckResponse>("user/check/", new{android_id = androidId});
            return data;
        }
        public static RegisterResponse CreateUser(string androidId, string _name)
        {
            RegisterResponse data = Net.Post<RegisterResponse>($"user/reg/", 
                new {android_id = androidId, name = _name});
            return data;
        }

        public static GetFriendLinksResponse GetFriendsList(string androidId)
        {
            GetFriendLinksResponse data = Net.Post<GetFriendLinksResponse>($"user/friend/list", new {android_id = androidId});
            return data;
        }

        public static UpdateFriendLinkResponse UpdateFriendStatus(string androidId, long userId, string _status)
        {
            UpdateFriendLinkResponse data = Net.Post<UpdateFriendLinkResponse>($"user/friend/update", 
                new {android_id = androidId, user_id = userId, status = _status});
            return data;
        }

        public static SendFriendLinkResponse offerFriendship(string androidId, long userId)
        {
            SendFriendLinkResponse data = Net.Post<SendFriendLinkResponse>("user/friend/send",
                new {android_id = androidId, user_id = userId});
            return data;
        }

        public static FindUsersResponse findUsers(string androidId, string _name)
        {
            FindUsersResponse data = Net.Post<FindUsersResponse>("user/friend/find",
                new {android_id = androidId, name = _name});
            return data;
        }
        
        

        /*
        public static LoginResponse LoginUser(string androidId)
        {
            LoginResponse data = Net.Get<LoginResponse>($"user/login/{androidId}");
            return data;
        }*/

        /*
        public static void ChangeNickname(string sessionId, string nickname)
        {
            Net.Post<ChangeNickname>($"User/change", new { session_id = sessionId, nickname = nickname});
        }
        */
    }
}
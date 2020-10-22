using System.Collections.Generic;
using AureoleCore.Models;

namespace AureoleCore.Responses
{
    public class RegisterResponse
    {
        public Error error { get; set; }
        //public User result { get; set; }
        
        public RegisterResponseData data { get; set; }

        public class RegisterResponseData
        {
            public long user_id { get; set; }
        }
    }

    public class Error
    {
        public int id { get; set; }
        public string message { get; set; }
    }

    public class CheckResponse
    {
        public Error error { get; set; }
        public CheckResponseData data { get; set; }

        public class CheckResponseData
        {
            public bool exists { get; set; }
        }
    }

    public class UpdateFriendLinkResponse
    {
        public Error error { get; set; }
        public UpdateFriendLinkResponseData data { get; set; }

        public class UpdateFriendLinkResponseData
        {
            public long link_id { get; set; }
        }
    }

    public class GetFriendLinksResponse
    {
        public Error error { get; set; }
        public GetFriendLinksResponseData data { get; set; }

        public class GetFriendLinksResponseData
        {
            public List<GetFriendLinksModel> links { get; set; }
        }

        public class GetFriendLinksModel
        {
            public string name { get; set; }
            public string status { get; set; }
            public long user_id { get; set; }
        }
    }

    public class FindUsersResponse
    {
        public Error error { get; set; }
        public FindUsersResponseData data { get; set; }

        public class FindUsersResponseData
        {
            public List<FindUserModel> users { get; set; }
        }

        public class FindUserModel
        {
            public long user_id { get; set; }
            public  string name { get; set; }
        }
    }
    
    public class SendFriendLinkResponse
    {
        public Error error { get; set; }
        public SendFriendLinkResponseData data { get; set; }

        public class SendFriendLinkResponseData
        {
            public long link_id { get; set; }
        }
    }
}

using System.Collections.Generic;

namespace AureoleCore.Models
{
    public class ChatModel
    {
        //public string Text { get; set; }
        
        public List<Message> messages { get; set; }
        
        public class Message
        {
            public string sender_name { get; set; }
            public string text { get; set; }
        }
    }

    public class ChatList
    {
        public List<Chat> chats { get; set; }
        
        public class Chat
        {
            public string name { get; set; }
        }
    }
}
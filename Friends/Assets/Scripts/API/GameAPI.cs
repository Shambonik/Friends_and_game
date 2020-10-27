using AureoleCore.Models;
using AureoleCore.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using UnityEngine.LowLevel;
using Random = System.Random;

namespace AureoleCore.API
{
    /// <summary>
    /// API для работы с игровым миром.
    /// </summary>
    public class GameAPI
    {
        public delegate void ChatUpdate();
        public event ChatUpdate ChatEvent;
        public delegate void InventoryUpdate();
        public event InventoryUpdate InventoryEvent;
        public static GameAPI gameAPI;

        public WorldUpdate WorldUpdate = new WorldUpdate();
        public static GameBuffer GameBuffer = new GameBuffer();
        
        //Передавать как аргумент???
        public static List<ChatModel> Chat = new List<ChatModel>();
        public static List<Item> InventoryItems = new List<Item>();
        
        //public static string inventory = "";

        private IPEndPoint host;

        public string Host
        {
            set { host = ParseEndPoint(value); }
        }

        private UdpClient UdpClient;
        private UdpState UdpState;


        private readonly float pingTime = 0;

        private int port;
        private string receiveString { get; set; }

        private static IPEndPoint ParseEndPoint(string value)
        {
            string[] temp = value.Split(':');

            return new IPEndPoint(IPAddress.Parse(temp[0]), int.Parse(temp[1]));
        }

        public void Connect(string sessionId, long unitId)
        {
            Start();
            Send(new {method = "connect", SessionId = sessionId, UnitId= unitId}, host);
        }

        public void CheckConnect()
        {
            Send(new {method = "connect2", key = GameBuffer.Key}, host);
        }

        private long lastPing;

        public void Ping()
        {
            lastPing = DateTime.Now.ToFileTime();
            Send(new {method = "ping"}, host);
        }

        public void Key(float x, float y)
        {
            Send(new {method = "keys", key = GameBuffer.Key, x = x, y = y, aid = GameBuffer.Aid}, host);
        }
        
        public void AddMessage(string text)
        {
            Send(new {method = "c_add", Text = text}, host);
        }

        public void ViewChatText()
        {
            Send(new {method = "c_get", Messages = Chat}, host);
        }

        private void Start()
        {
            port = new Random().Next(10000, 11111);
            UdpClient = new UdpClient(port);
            UdpState = new UdpState() {Udp = UdpClient};
            Ping();
            UdpClient.BeginReceive(RecieveCallBack, UdpState);
        }

        private void RecieveCallBack(IAsyncResult r)
        {
            UdpState state = r.AsyncState as UdpState;

            UdpClient u = state.Udp;
            IPEndPoint e = state.EndPoint;

            try
            {
                byte[] receiveBytes = u.EndReceive(r, ref e);
                receiveString = Encoding.UTF8.GetString(receiveBytes);
                new System.Threading.Tasks.Task(() => { ClientUpdate(); }).Start();
            }
            catch (Exception)
            {
                u.Close();
                u = new UdpClient(port);
                UdpState = new UdpState() {Udp = UdpClient};
            }
            finally
            {
                u.BeginReceive(RecieveCallBack, state);
            }
        }
        
        private List<long> packagesTime = new List<long>();
        
        //hardcode chat (beginning)
        public static void GetPublicChat()
        {
            string jsonString = "{\"messages\" : [{\"sender_name\" : \"Dan\", \"text\" : \"asdadsafsafaffsfsaf\"}," +
                                "{\"sender_name\" : \"Shamb\", \"text\" : \"another text\"}," +
                                "{\"sender_name\" : \"Leha\", \"text\" : \"Zdarova\"}]}";
            ChatModel chat = JsonConvert.DeserializeObject<ChatModel>(jsonString);
            setChat(chat);
        }
        
        public static void GetChatWithPlayer()
        {
            string jsonString = "{\"messages\":[{\"sender_name\" : \"Player\", \"text\" : \"ti loh\"}," +
                                "{\"sender_name\" : \"Me\", \"text\" : \"net ti\"}]}";
            ChatModel chat = JsonConvert.DeserializeObject<ChatModel>(jsonString);
            setChat(chat);
        }

        public static void GetPrivateChatList()
        {
            string jsonString = "{\"chats\" : [{\"name\" : \"Player\"}," +
                                "{\"name\" : \"Leha\"}]}";
            ChatList chatList = JsonConvert.DeserializeObject<ChatList>(jsonString);
            setChatList(chatList);
        }

        private static void setChatList(ChatList chatList)
        {
            ChatPanelScript.chat.setPrivateChatsList(chatList);
        }

        private static void setChat(ChatModel chat)
        {
            ChatPanelScript.chat.setChat(chat);
        }
        //hardcode chat (end)


        private void ClientUpdate()
        {
            
            var serverObj = JObject.Parse(receiveString);
            switch (serverObj.SelectToken("Method").ToString())
            {
                case "connect":
                    GameBuffer = JsonConvert.DeserializeObject<GameBuffer>(receiveString);
                    CheckConnect();
                    break;
                case "pong":
                    GameBuffer.Ping = (int) (DateTime.Now.ToFileTime() - lastPing)/10000;
                    Debug.Log("Ping " + GameBuffer.Ping);
                    break;
                case "update":
                    WorldUpdate = JsonConvert.DeserializeObject<WorldUpdate>(receiveString);
                    while ((packagesTime.Count > 0) && (DateTime.Now.ToFileTime() - packagesTime[0] > 10000000))
                    {
                        packagesTime.RemoveAt(0);
                    }
                    packagesTime.Add(DateTime.Now.ToFileTime());
                    Debug.Log("Package " + (packagesTime.Count/0.2) + '%');
                    break;
                case "c_get":
                    string messages = JObject.Parse(receiveString).SelectToken("Messages").ToString();
                    Chat = JsonConvert.DeserializeObject<List<ChatModel>>(messages);
                    ChatEvent?.Invoke();
                    break;
                case "get_inventory":
                    string inventory = JObject.Parse(receiveString).SelectToken("Body").ToString();
                    InventoryItems = JsonConvert.DeserializeObject<List<Item>>(inventory);
                    InventoryEvent?.Invoke();
                    break;
            }
            
        }


        protected void Send(object message, IPEndPoint endPoint)
        {
            Send(JsonConvert.SerializeObject(message), endPoint);
        }

        protected void Send(string message, IPEndPoint endPoint)
        {
            byte[] buf = Encoding.UTF8.GetBytes(message);
            UdpClient.Send(buf, buf.Length, endPoint);
        }


        public void equipItem(long itemId)
        {
            Send(new {method = "equip_item", key = GameBuffer.Key, id = itemId}, host);
        }
        
        public void unequipItem(long itemId)
        {
            Send(new {method = "unequip_item", key = GameBuffer.Key, id = itemId}, host);
        }

        public void takeItem(long itemId)
        {
            Send(new {method = "take_item", key = GameBuffer.Key, id = itemId}, host);
        }

        public void getInventory()
        {
            Send(new {method = "get_inventory", key = GameBuffer.Key}, host);
        }
        
        public static void SendAttack()
        {
            GameBuffer.Aid = -1;
        }
    }
}
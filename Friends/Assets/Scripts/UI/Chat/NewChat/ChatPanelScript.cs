using System.Collections;
using System.Collections.Generic;
using AureoleCore.API;
using AureoleCore.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChatPanelScript : MonoBehaviour
{

    [SerializeField] private Transform content;
    [SerializeField] private GameObject messagePanel;
    [SerializeField] private GameObject chatButton;

    public static ChatPanelScript chat;

    public void openFriends()
    {
        SceneManager.LoadScene("Friends");
    }

    private void clearContent()
    {
        foreach (var obj in content.gameObject.GetComponentsInChildren<Transform>())
        {
            if(obj!=content) Destroy(obj.gameObject);
        }
    }

    public void setPrivateChatsList(ChatList chatList)
    {
        clearContent();
        foreach (var chat in chatList.chats)
        {
            var chatObj = Instantiate(chatButton, content);
            chatObj.GetComponent<ChatButtonScript>().setData(chat.name, this);
        }
    }

    public void openPrivateChatsList()
    {
        GameAPI.GetPrivateChatList();
    }

    public void openPublicChat()
    {
        GameAPI.GetPublicChat();
    }

    public void setChat(ChatModel chat)
    {
        clearContent();
        foreach (var mess in chat.messages)
        {
            var messageObj = Instantiate(messagePanel, content);
            messageObj.GetComponent<MessagePanelScript>().setData(mess.sender_name, mess.text);
        }
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        chat = this;
        openPublicChat();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

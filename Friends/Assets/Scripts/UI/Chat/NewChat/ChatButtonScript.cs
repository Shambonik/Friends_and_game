using System.Collections;
using System.Collections.Generic;
using AureoleCore.API;
using UnityEngine;
using UnityEngine.UI;

public class ChatButtonScript : MonoBehaviour
{
    [SerializeField] private Text name;
    private ChatPanelScript _chatPanelScript;

    public void setData(string text, ChatPanelScript chatPanelScript)
    {
        name.text = text;
        _chatPanelScript = chatPanelScript;
    }

    public void onClick()
    {
        GameAPI.GetChatWithPlayer();
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

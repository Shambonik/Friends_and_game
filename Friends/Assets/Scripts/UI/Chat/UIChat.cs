using System;
using System.Collections;
using _Games.Scripts.Netwotking;
using AureoleCore.API;
using UnityEngine;
using UnityEngine.UI;

public class UIChat : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Text console;
    private Animator anim;
    private bool isActive = false;
    
    public void Chat()
    {
        if (!isActive)
        {
            isActive = true;
            anim.Play("ChatOpen");
        }
        else
        {
            isActive = false;
            anim.Play("ChatClose");
        }
    }
    
    private void Start()
    {
        StartCoroutine(ConsoleUpdate());
        anim = GetComponent<Animator>();
        //GameAPI.gameAPI.ChatEvent += ChatUpdate;
        GameAPI.gameAPI.Connect("sfsf", 34);
    }

    public void EnterMessage()
    {
        GameAPI.gameAPI.AddMessage(inputField.text);
        inputField.text = "";
    }
    
    private IEnumerator ConsoleUpdate()
    {
        while (true)
        {
            //Debug.Log($"Check activated: {IsActrive}");
            if (GameAPI.gameAPI != null && isActive)
                GameAPI.gameAPI.ViewChatText();

            yield return new WaitForSeconds(0.1f);
        }
    }

    /*private void ChatUpdate()
    {
        Debug.Log("CHAT_IVENT " + console.text);
        console.text = "";
        foreach (var text in GameAPI.Chat)
        {
            console.text = console.text + "{" + text.Text + "}\n";
        }
    }*/
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MessageScript : MonoBehaviour
{
    private Text messageText;
    private Queue<string> queueMessages = new Queue<string>();
    private float lastTime = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        messageText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if ((messageText.text=="")||(Time.time - lastTime > 5))
        {
            lastTime = Time.time;
            if (queueMessages.Count > 0) messageText.text = queueMessages.Dequeue();
            else messageText.text = "";
        }
    }

    public void addMessage(string message)
    {
        queueMessages.Enqueue(message);
    }

}

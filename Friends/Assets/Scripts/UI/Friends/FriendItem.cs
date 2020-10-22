using System.Collections;
using System.Collections.Generic;
using AureoleCore.UserAPI;
using UnityEngine;
using UnityEngine.UI;

public class FriendItem : MonoBehaviour
{

    private string status;
    private long user_id;
    private string name;
    [SerializeField] private Text text;
    [SerializeField] private GameObject addButton;
    [SerializeField] private GameObject deleteButton;
    

    public void setData(string status, long user_id, string name)
    {
        this.status = status;
        this.user_id = user_id;
        this.name = name;
        text.text = name + " " + status;
        if (status == "friend" || status == "sent")
        { 
            addButton.SetActive(false);
            deleteButton.SetActive(true);
        }
        else if (status == "expects")
        {
            addButton.SetActive(true);
            deleteButton.SetActive(true);
        }
        else if (status == "null")
        {
            addButton.SetActive(true);
            deleteButton.SetActive(false);
        }
        
    }

    public void onAdd()
    {
        if (status == "null") UserAPI.offerFriendship(DeviceInfo.AnroidID(), user_id);
        else if (status == "expects") UserAPI.UpdateFriendStatus(DeviceInfo.AnroidID(), user_id, "accept");
        addButton.SetActive(false);
        deleteButton.SetActive(true);
    }

    public void onDelete()
    {
        if (status == "friend" || status == "expects" || status == "sent") UserAPI.UpdateFriendStatus(DeviceInfo.AnroidID(), user_id, "decline");
        addButton.SetActive(true);
        deleteButton.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        text = transform.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using AureoleCore.UserAPI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FriendsScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject panel;
    [SerializeField] private Transform content;
    [SerializeField] private InputField input;
    void Start()
    {
        getFriends();
    }

    public void getFriends()
    {
        Debug.Log(content);
        foreach (var obj in content.gameObject.GetComponentsInChildren<Transform>())
        {
            if(obj!=content) Destroy(obj.gameObject);
        }
        var friendsList = UserAPI.GetFriendsList(DeviceInfo.AnroidID());
        foreach (var friend in friendsList.data.links)
        {
            var friendPanel = Instantiate(panel, content);
            FriendItem script = friendPanel.GetComponent<FriendItem>();
            script.setData(friend.status, friend.user_id, friend.name);
        }
    }

    public void findFriends()
    {
        foreach (var obj in content.gameObject.GetComponentsInChildren<Transform>())
        {
            if(obj!=content) Destroy(obj.gameObject);
        }
        var usersList = UserAPI.findUsers(DeviceInfo.AnroidID(), input.text);
        foreach (var user in usersList.data.users)
        {
            var friendPanel = Instantiate(panel, content);
            FriendItem script = friendPanel.GetComponent<FriendItem>();
            script.setData("null", user.user_id, user.name);
        }
    }

    public void openChat()
    {
        SceneManager.LoadScene("Chat");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

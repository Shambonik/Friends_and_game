using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AureoleCore.API;
using UnityEngine;
using UnityEngine.UI;

public class ItemMenu : MonoBehaviour
{

    [SerializeField] private Text button;
    private static List<KeyValuePair<long, string>> items = new List<KeyValuePair<long, string>>();

    private Network network;
    
    void Start()
    {
        network = FindObjectOfType<Network>();
    }

    public static void newList()
    {
        items = new List<KeyValuePair<long, string>>();
    }

    public static void addItem(long id, string item)
    {
        items.Add(new KeyValuePair<long, string>(id, item));
    }

    private void getItems()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        getItems();
        if (items != null && items.Count > 0)
        {
            button.GetComponent<Text>().text = items[0].Value;
        }
        else
        {
            button.GetComponent<Text>().text = "Shmotok netъ";
        }
    }

    public void takeItem()
    {
        if (items != null && items.Count > 0)
        {
            GameAPI.gameAPI.takeItem(items[0].Key);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using AureoleCore.API;
using UnityEngine;
using UnityEngine.UI;

public class InventoryScript : MonoBehaviour
{
    
    [SerializeField] private GameObject inventory;
    private Transform content;
    [SerializeField] private GameObject itemButton;
    
    private Dictionary<long, GameObject> inventoryItems = new Dictionary<long, GameObject>();

    //private Text inventoryText;

    // Start is called before the first frame update
    void Start()
    {
        //inventoryText = inventory.GetComponentInChildren<Text>();
        content = transform.Find("content");
        GameAPI.gameAPI.InventoryEvent += setInventory;
    }

    // Update is called once per frame
    void Update()
    {
        //inventoryText.text = GameAPI.inventory;
    }
    
    
    public void setInventory()
    {
        Dictionary<long, GameObject>  inventoryItemsOld = inventoryItems;
        inventoryItems = new Dictionary<long, GameObject>();
        foreach (var item in GameAPI.InventoryItems)
        {
            if (inventoryItemsOld.ContainsKey(item.ItemId))
            {
                inventoryItems.Add(item.ItemId, inventoryItemsOld[item.ItemId]);
                inventoryItemsOld.Remove(item.ItemId);
            }
            else
            {
                var itemObj = Instantiate(itemButton, content);
                itemObj.GetComponent<ItemInventory>().Create(item.ItemId, item.ItemName, item.isEquipped);
                inventoryItems.Add(item.ItemId, itemObj);
            }
        }

        foreach (var item in inventoryItemsOld)
        {
            Destroy(item.Value);
        }
    }


    public void openInventory()
    {
        inventory.SetActive(true);
        GameAPI.gameAPI.getInventory();
    }

    public void closeInventory()
    {
        inventory.SetActive(false);
    }
}

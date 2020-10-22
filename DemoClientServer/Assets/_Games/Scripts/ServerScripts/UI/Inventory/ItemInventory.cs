using System.Collections;
using System.Collections.Generic;
using AureoleCore.API;
using UnityEngine;
using UnityEngine.UI;

public class ItemInventory : MonoBehaviour
{

    private long itemId;
    private bool equipped;
    private Button _button;

    public void Create(long itemId, string itemName, bool equipped)
    {
        this.itemId = itemId;
        this.equipped = equipped;
        _button = transform.GetComponentInChildren<Button>();
        _button.GetComponentInChildren<Text>().text = (equipped)?"Снять":"Надеть";
        _button.onClick.AddListener(equippedChange);
        transform.Find("ItemName").GetComponent<Text>().text =  itemName;
    }

    public void equippedChange()
    {
        if (equipped)
        {
            equipped = false;
            _button.GetComponentInChildren<Text>().text = "Надеть";
            GameAPI.gameAPI.unequipItem(itemId);
        }
        else
        {
            equipped = true;
            _button.GetComponentInChildren<Text>().text = "Снять";
            GameAPI.gameAPI.equipItem(itemId);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using AureoleCore.UnitAPI;
using AureoleCore.UserAPI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UnitsScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject button;
    [SerializeField] private Transform content;
    [SerializeField] private InputField input;
    [SerializeField] private Text unitText;
    private UnitItem unit;

    public void setUnit(UnitItem unit)
    {
        this.unit = unit;
        unitText.text = "Выбран персонаж " + unit.getName();
    }
    void Start()
    {
        getUnits();
    }

    public void getUnits()
    {
        //Debug.Log(content);
        foreach (var obj in content.gameObject.GetComponentsInChildren<Transform>())
        {
            if(obj!=content) Destroy(obj.gameObject);
        }
        var unitsList = UnitAPI.GetUserUnits(DeviceInfo.AnroidID());
        foreach (var unit in unitsList.data.units)
        {
            var unitButton = Instantiate(button, content);
            var script = unitButton.GetComponent<UnitItem>();
            script.setData(unit.id, unit.name,unit.class_name,unit.hp,unit.mana,unit.level, unit.skill_points, this);
        }
    }

    public void createCharacter()
    {
        UnitAPI.CreateNewUnit(DeviceInfo.AnroidID(), input.text, "caetus");
        getUnits();
    }
    
    
    

    /*public void openChat()
    {
        SceneManager.LoadScene("Chat");
    }*/

    // Update is called once per frame
    void Update()
    {
        
    }
}

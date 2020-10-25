using System.Collections;
using System.Collections.Generic;
using AureoleCore.UserAPI;
using UnityEngine;
using UnityEngine.UI;

public class UnitItem : MonoBehaviour
{
    private long id;
    private string name;
    private string class_name;

    private float hp;
    private float mana;

    private int level;
    private int skill_points;
    [SerializeField] private Text text;
    private UnitsScript _unitsScript;
    

    public void setData(long id, string name, string class_name, float hp, float mana, int level, int skill_points, UnitsScript unitsScript)
    {
        this.id = id;
        this.name = name;
        this.class_name = class_name;
        this.hp = hp;
        this.mana = mana;
        this.level = level;
        this.skill_points = skill_points;
        _unitsScript = unitsScript;
        text.text = name + ": " + class_name + "\nlvl " + level;
    }

    public string getName()
    {
        return name;
    }



    public void onClick()
    {
        _unitsScript.setUnit(this);
    }
    
    
    // Start is called before the first frame update
    void Start()
    {
        //text = transform.GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

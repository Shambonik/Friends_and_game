using System;
using System.Collections;
using System.Collections.Generic;
using UnitController;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public EnemyPanel EnemyPanel;
    public CharacterPanel CharacterPanel;
    
    public static UIManager Instance;

    private void Awake()
    {
        if (!Instance) 
            Instance = this;
        else 
            Destroy(gameObject);
    }
}

[Serializable]
public class EnemyPanel
{
    public GameObject Panel;
    
    public Image HealthBar;
    public Image Avatar;
    
    public Text Title;
    public Text HealthPoint;
    public Text Lvl;
}

[Serializable]
public class CharacterPanel
{
    public GameObject Panel;
    
    public Image HealthBar;
    public Image ManaBar;
    public Image ExpBar;
    public Image Avatar;
    
    public Text Title;
    public Text HealthPoint;
    public Text ManaPoint;
    public Text Lvl;
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

public class CharacterData : UnitData
{
    private CharacterPanel CharacterPanel;
    public float UnitLvlProgress = 0;

    private void Start()
    {
        CharacterPanel = UIManager.Instance.CharacterPanel;
        hpBar = CharacterPanel.HealthBar;
        hpPoint = CharacterPanel.HealthPoint;
        ShowHp();
        OnRegen += ShowHp;
    }

    private void ShowHp()
    {
        hpBar.fillAmount = (float) Stats.CurrentHealth / Stats.stats.health;
        hpPoint.text = $"{Stats.CurrentHealth}/{Stats.stats.health}";
    }
    public void TakeExp(int ammount)
    {
        UnitLvlProgress += ammount;
        float progress = UnitLvlProgress / (UnitLvl * 137);
        CharacterPanel.ExpBar.fillAmount = progress;
        
        if (UnitLvlProgress >= UnitLvl * 137)
        {
            UnitLvlProgress = 0;
            CharacterPanel.ExpBar.fillAmount = 0;
            UnitLvl++;
            Stats.stats.health += 10 * UnitLvl;
            Stats.CurrentHealth = Stats.stats.health;
            CharacterPanel.Lvl.text = $"Ур: {UnitLvl}";
        }
    }
}

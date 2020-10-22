using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

public class EnemyData : UnitData
{
    private EnemyPanel _enemyPanel;

    private void Start()
    {
        _enemyPanel = UIManager.Instance.EnemyPanel;
        hpBar = _enemyPanel.HealthBar;
        hpPoint = _enemyPanel.HealthPoint;
    }
}

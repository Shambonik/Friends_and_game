using System.Collections;
using System.Collections.Generic;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.UI;

public class OutputSystem : MonoBehaviour
{
    [SerializeField] private Image HpBar;
    [SerializeField] private Text HpText;
    [SerializeField] private Image ManaBar;
    [SerializeField] private Text ManaText;
    [SerializeField] private Image ExpBar;
    [SerializeField] private Character player;
    public static OutputSystem _outputSystem;
    

    public void UpdateData()
    {
        HpBar.fillAmount = player.Health / player.MAXHealth;
        HpText.text = player.Health + "/" + player.MAXHealth;
        ManaBar.fillAmount = player.Mana / player.MAXMana;
        ManaText.text = player.Mana + "/" + player.MAXMana;
        ExpBar.fillAmount = player.Exp / player.MAXExp;
    }

    // Start is called before the first frame update
    void Start()
    {
        _outputSystem = this;
        UpdateData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

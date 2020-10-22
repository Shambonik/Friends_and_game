using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

public class MannequinInit : MonoBehaviour
{
    // Start is called before the first frame update
    int m_HitAnimHash = Animator.StringToHash("Hit");
    void Start()
    {
        var m_CharacterData = GetComponent<UnitData>();
        m_CharacterData.Init();
            
        m_CharacterData.OnDamage += () =>
        {
            GetComponent<Animator>().SetTrigger(m_HitAnimHash);
        };
    }

}

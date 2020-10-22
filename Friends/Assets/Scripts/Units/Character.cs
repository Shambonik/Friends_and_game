using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Unit
{

    private int attackCount = 0;
    private bool canAttack = true;
    private float timeLastAttack = 0;


    void Update()
    {
        AttackCount();
        changePosition(); 
        changeRotation();
    }

    public override void AttackAnimation()
    {
        if (canAttack)
        {
            StartCoroutine(CanAttack());
            attackCount++;
            if (animator != null) animator.SetInteger("AttackCount", attackCount % 4);
            timeLastAttack = Time.time;
        }
    }
    
    private IEnumerator CanAttack()
    {
        canAttack = false;
        yield return new WaitForSeconds(0.5f);
        canAttack = true;
    }

    private void AttackCount()
    {
        if (Time.time - timeLastAttack > 1)
        {
            attackCount = 0;
            if (animator != null) animator.SetInteger("AttackCount", attackCount);
        }
    }
}

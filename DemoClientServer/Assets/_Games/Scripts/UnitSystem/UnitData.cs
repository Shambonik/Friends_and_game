using System;
using System.Collections;
using System.Collections.Generic;
using CreatorKitCode;
using CreatorKitCodeInternal;
using UnityEngine;
using UnityEngine.UI;

namespace UnitSystem
{
    public class UnitData : MonoBehaviour
    {
        public string CharacterName;
        public int UnitLvl = 1;
        private int ExpCount = 1;
        public StatSystem Stats;

        public Sprite Avatar;
        public Weapon StartingWeapon;
        public InventorySystem Inventory = new InventorySystem();
        public EquipmentSystem Equipment = new EquipmentSystem();
        
        [HideInInspector]
        public Image hpBar;
        [HideInInspector]
        public Text hpPoint;
        public Action OnDamage { get; set; }
        public Action OnRegen { get; set; }
        
        public bool CanAttack
        {
            get { return m_AttackCoolDown <= 0.0f; }
        }
        
        float m_AttackCoolDown;
        private float m_HitCoolDown;

        public void Init()
        {
            Stats.Init(this);
            Inventory.Init(this);
            Equipment.Init(this);

            if (StartingWeapon != null)
            {
                StartingWeapon.UsedBy(this);
                Equipment.InitWeapon(StartingWeapon, this);
            }
        }

        private bool regenBlock;
        void Update()
        {
            if (m_AttackCoolDown > 0.0f)
                m_AttackCoolDown -= Time.deltaTime;
            
            if (m_HitCoolDown > 0.0f)
                m_HitCoolDown -= Time.deltaTime;
            else if (!regenBlock)
            {
                regenBlock = true;
                StartCoroutine(AutoRegen());
            }
        }

        public int Exp()
        {
            ExpCount *= UnitLvl * 10;
            return ExpCount;
        }
        public bool CanAttackReach(UnitData target)
        {
            return Equipment.Weapon.CanHit(this, target);
        }
        
        public bool CanAttackTarget(UnitData target)
        {
            if (!CanAttackReach(target))
                return false;

            if (m_AttackCoolDown > 0.0f)
                return false;

            return true;
        }

        public bool TargetIsLive(UnitData target)
        {
            if (target.Stats.CurrentHealth == 0)
                return false;

            return true;
        }
   
        public void Attack(UnitData target)
        {
            Equipment.Weapon.Attack(this, target);
        }
        
        public void AttackTriggered()
        {
            m_HitCoolDown = 5f;
            regenBlock = false;
            m_AttackCoolDown = Equipment.Weapon.Stats.Speed - (Stats.stats.agility * 0.5f * 0.001f * Equipment.Weapon.Stats.Speed);
        }

        public void Damage(Weapon.AttackData attackData)
        {
            m_HitCoolDown = 5f;
            regenBlock = false;
            Stats.Damage(attackData);
            OnDamage?.Invoke();
        }

        private IEnumerator AutoRegen()
        {
            while (Stats.CurrentHealth < Stats.stats.health && regenBlock)
            {
                Stats.CurrentHealth++;
                OnRegen?.Invoke();
                yield return new WaitForSeconds(1);
            }
        }
    }
}


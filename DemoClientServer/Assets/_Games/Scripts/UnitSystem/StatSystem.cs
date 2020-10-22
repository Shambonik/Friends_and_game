using System;
using System.Collections;
using System.Collections.Generic;
using CreatorKitCode;
using UnitSystem;
using UnityEditor;
using UnityEngine;

namespace UnitSystem
{
    /// <summary>
    /// Этот класс хранит в себе различные атрибуты (например, здоровье, сила и прочее),
    /// а так же содержит различный функционал для взаимодействия с жтими атрибутами.
    /// </summary>
    [Serializable]
    public class StatSystem
    {
        /// <summary>
        /// Все типы урона, которые есть в игре
        /// </summary>
        public enum DamageType
        {
            Physical
        }
        
        /// <summary>
        /// Этот класс содержит различные атрибуты, а также простую защиту и усиление
        /// для каждого типа урона
        /// </summary>
        [Serializable]
        public class Stats
        {
            public int health;
            public int defense;
            public int mana;
            public int strength;
            public int agility;
            public int intelligence;
            
            public int[] elementalProtection = new int[Enum.GetValues(typeof(DamageType)).Length];   
            public int[] elementalBoosts = new int[Enum.GetValues(typeof(DamageType)).Length];
            
            public void Copy(Stats other)
            {
                health = other.health;
                defense = other.defense;
                mana = other.mana;
                strength = other.strength;
                intelligence = other.intelligence;
            
                Array.Copy(other.elementalProtection, elementalProtection, other.elementalProtection.Length);
                Array.Copy(other.elementalBoosts, elementalBoosts, other.elementalBoosts.Length);
            }
        }
        
        public Stats baseStats;
        public Stats stats { get; set; } = new Stats();
        public int CurrentHealth { get; set; }
        
        UnitData m_Owner;
        
        public void Init(UnitData owner)
        {
            stats.Copy(baseStats);
            CurrentHealth = stats.health;
            m_Owner = owner;
        }
        
        public void ChangeHealth(int amount)
        {
            CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, stats.health);
            float fillAmount = (float)CurrentHealth / stats.health;
            m_Owner.hpBar.fillAmount = fillAmount;
            m_Owner.hpPoint.text = $"{CurrentHealth}/{stats.health}";
        }
        
        public void Damage(Weapon.AttackData attackData)
        {
            int totalDamage = attackData.GetFullDamage();
        
            ChangeHealth(-totalDamage);
            //DamageUI.Instance.NewDamage(totalDamage, m_Owner.transform.position);
        }
    }
}



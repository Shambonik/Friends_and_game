using System.Collections.Generic;
using CreatorKitCode;
using UnitSystem;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
#if UNITY_EDITOR
using UnityEditor;
using System.Linq;
#endif

namespace CreatorKitCode 
{
    public class Weapon : EquipmentItem
    {
        public class AttackData
        {
            public UnitData Target => m_Target;
            public UnitData Source => m_Source;
            
            UnitData m_Target;
            UnitData m_Source;
        
            int[] m_Damages = new int[System.Enum.GetValues(typeof(StatSystem.DamageType)).Length];
           
            public AttackData(UnitData target, UnitData source = null)
            {
                m_Target = target;
                m_Source = source;
            }

            public int AddDamage(StatSystem.DamageType damageType, int amount)
            {
                int addedAmount = amount;

                if (damageType == StatSystem.DamageType.Physical)
                {
                    if(m_Source !=  null)
                        addedAmount += Mathf.FloorToInt(addedAmount * m_Source.Stats.stats.strength * 0.01f);
                
                    addedAmount = Mathf.Max(addedAmount - m_Target.Stats.stats.defense, 1);
                }
            
                if(m_Source != null)
                    addedAmount += addedAmount * Mathf.FloorToInt(m_Source.Stats.stats.elementalBoosts[(int)damageType] / 100.0f);
            
                addedAmount -= addedAmount * Mathf.FloorToInt(m_Target.Stats.stats.elementalProtection[(int)damageType] / 100.0f);
            
                m_Damages[(int)damageType] += addedAmount;

                return addedAmount;
            }

            public int GetDamage(StatSystem.DamageType damageType)
            {
                return m_Damages[(int)damageType];
            }

            public int GetFullDamage()
            {
                int totalDamage = 0;
                for (int i = 0; i < m_Damages.Length; ++i)
                {
                    totalDamage += m_Damages[i];
                }

                return totalDamage;
            }
        }
    
        [System.Serializable]
        public struct Stat
        {
            public float Speed;
            public int MinimumDamage;
            public int MaximumDamage;
            public float MaxRange;
        }

        /*[Header("Sounds")]
        public AudioClip[] HitSounds;
        public AudioClip[] SwingSounds;*/
    
        [Header("Stats")]
        public Stat Stats = new Stat(){ Speed = 1.0f, MaximumDamage = 1, MinimumDamage = 1};

        public void Attack(UnitData attacker, UnitData target)
        {
            AttackData attackData = new AttackData(target, attacker);

            int damage = Random.Range(Stats.MinimumDamage, Stats.MaximumDamage + 1);

            attackData.AddDamage(StatSystem.DamageType.Physical, damage);
            
            target.Damage(attackData);
        }
        
        public bool CanHit(UnitData attacker, UnitData target)
        {
            if (Vector3.SqrMagnitude(attacker.transform.position - target.transform.position) < Stats.MaxRange * Stats.MaxRange)
            {
                return true;
            }

            return false;
        }

        public override string GetDescription()
        {
            string desc = base.GetDescription();

            int minimumDPS = Mathf.RoundToInt(Stats.MinimumDamage / Stats.Speed);
            int maximumDPS = Mathf.RoundToInt(Stats.MaximumDamage / Stats.Speed);

            desc += "\n";
            desc += $"Damage: {Stats.MinimumDamage} - {Stats.MaximumDamage}\n";
            desc += $"DPS: {minimumDPS} - {maximumDPS}\n";
            desc += $"Attack Speed : {Stats.Speed}s\n";

            return desc;
        }

        /*public AudioClip GetHitSound()
        {
            if (HitSounds == null || HitSounds.Length == 0)
                return SFXManager.GetDefaultHit();

            return HitSounds[Random.Range(0, HitSounds.Length)];
        }
    
        public AudioClip GetSwingSound()
        {
            if (SwingSounds == null || SwingSounds.Length == 0)
                return SFXManager.GetDefaultSwingSound();

            return SwingSounds[Random.Range(0, SwingSounds.Length)];
        }*/
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Weapon))]
public class WeaponEditor : Editor
{
    [MenuItem("Assets/Create/Beginner Code/Weapon", priority = -999)]
    static public void CreateWeapon()
    {
        var newWeapon = CreateInstance<Weapon>();
        newWeapon.Slot = EquipmentItem.EquipmentSlot.Weapon;
        
        ProjectWindowUtil.CreateAsset(newWeapon, "weapon.asset");
    }
}
#endif
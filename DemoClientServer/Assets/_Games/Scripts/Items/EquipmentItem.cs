using System.Collections.Generic;
using UnityEngine;
using UnitSystem;

namespace CreatorKitCode 
{
    [CreateAssetMenu(fileName = "EquipmentItem", menuName = "Beginner Code/Equipment Item", order = -999)]
    public class EquipmentItem : Item
    {
        public enum EquipmentSlot
        {
            Weapon
        }
        
        public EquipmentSlot Slot;
        
        [Header("Minimum Stats")]
        public int MinimumStrength;
        public int MinimumAgility;
        
        public override bool UsedBy(UnitData user)
        {
            var userStat = user.Stats.stats;

            if (userStat.agility < MinimumAgility
                || userStat.strength < MinimumStrength)
            {
                return false;
            }

            user.Equipment.Equip(this);
        
            return true;
        }

        public override string GetDescription()
        {
            string desc = base.GetDescription();

        
            bool requireStrength = MinimumStrength > 0;
            bool requireAgility = MinimumAgility > 0;

            if (requireStrength || requireAgility)
            {
                desc += "\nRequire : \n";

                if (requireStrength)
                    desc += $"Strength : {MinimumStrength}";

                if (requireAgility)
                {
                    if (requireStrength || requireAgility) desc += " & ";
                    desc += $"Agility : {MinimumAgility}";
                }
                
            }

            return desc;
        }
    }
}
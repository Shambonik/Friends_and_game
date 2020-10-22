using System.Collections.Generic;
using System.Linq;
using CreatorKitCode;
using UnityEngine;
using UnitSystem;

namespace CreatorKitCode {
   
    [CreateAssetMenu(fileName = "UsableItem", menuName = "Beginner Code/Usable Item", order = -999)]
    public class UsableItem : Item
    {
        public abstract class UsageEffect : ScriptableObject
        {
            public string Description;
            public abstract bool Use(UnitData user);
        }

        public List<UsageEffect> UsageEffects;

        public override bool UsedBy(UnitData user)
        {
            bool wasUsed = false;
            foreach (var effect in UsageEffects)
            {
                wasUsed |= effect.Use(user);
            }
        
            return wasUsed;
        }

        public override string GetDescription()
        {
            string description = base.GetDescription();
        
            if(!string.IsNullOrWhiteSpace(description))
                description += "\n";
            else
                description = "";

        
            foreach (var effect in UsageEffects)
            {
                description += effect.Description + "\n";
            }

            return description;
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnitSystem;
using UnityEngine;

namespace CreatorKitCode 
{
    public class EquipmentSystem
    {
        public Weapon Weapon { get; private set; }

        public Action<EquipmentItem> OnEquiped { get; set; }
        public Action<EquipmentItem> OnUnequip { get; set; }

        UnitData m_Owner;
        
        Weapon m_DefaultWeapon;

        public void Init(UnitData owner)
        {
            m_Owner = owner;
        }
        
        public void InitWeapon(Weapon wep, UnitData data)
        {
            m_DefaultWeapon = wep;
        }
        
        public void Equip(EquipmentItem item)
        {
            Unequip(item.Slot, true);

            OnEquiped?.Invoke(item);

            if (item.Slot == (EquipmentItem.EquipmentSlot.Weapon))
            {
                Weapon = item as Weapon;
            }
        }

        public void Unequip(EquipmentItem.EquipmentSlot slot, bool isReplacement = false)
        {
            if (slot == (EquipmentItem.EquipmentSlot.Weapon))
            {
                if (Weapon != null &&
                    (Weapon != m_DefaultWeapon || isReplacement)
                ) // the only way to unequip the default weapon is through replacing it
                {
                    //the default weapon does not go back to the inventory
                    if (Weapon != m_DefaultWeapon)
                        m_Owner.Inventory.AddItem(Weapon);

                    OnUnequip?.Invoke(Weapon);
                    Weapon = null;

                    //reequip the default weapon if this is not an unequip to equip a new one
                    if (!isReplacement)
                        Equip(m_DefaultWeapon);
                }
            }
        }

    }
}
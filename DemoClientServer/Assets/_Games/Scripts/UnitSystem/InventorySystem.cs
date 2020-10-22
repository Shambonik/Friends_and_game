using UnitSystem;
using UnityEngine;

namespace CreatorKitCode 
{
    public class InventorySystem
    {
        public class InventoryEntry
        {
            public int Count;
            public Item Item;
        }

        public InventoryEntry[] Entries = new InventoryEntry[32];

        UnitData m_Owner;
        
        public void Init(UnitData owner)
        {
            m_Owner = owner;
        }
        
        public void AddItem(Item item)
        {
            bool found = false;
            int firstEmpty = -1;
            for (int i = 0; i < 32; ++i)
            {
                if (Entries[i] == null)
                {
                    if (firstEmpty == -1)
                        firstEmpty = i;
                }
                else if (Entries[i].Item == item)
                {
                    Entries[i].Count += 1;
                    found = true;
                }
            }

            if (!found && firstEmpty != -1)
            {
                InventoryEntry entry = new InventoryEntry();
                entry.Item = item;
                entry.Count = 1;

                Entries[firstEmpty] = entry;
            }
        }

        public bool UseItem(InventoryEntry item)
        {
            if (item.Item.UsedBy(m_Owner))
            {
                item.Count -= 1;

                if (item.Count <= 0)
                {
                    for (int i = 0; i < 32; ++i)
                    {
                        if (Entries[i] == item)
                        {
                            Entries[i] = null;
                            break;
                        }
                    }
                }
                return true;
            }
            return false;
        }
    }
}
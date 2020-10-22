using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AureoleCore.Models
{
    public class WorldUpdate
    {
        public List<UnitsUpdate> Players { get; set; }
        public List<UnitsUpdate> Units { get; set; }
        public List<EventUpdate> Events { get; set; }
        
        public List<ItemUpdate> Items { get; set; }
    }

    public class UnitsUpdate
    {
        public long Id { get; set; }
        public string ModelInfo { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Dx { get; set; }
        public float Dy { get; set; }
        public float Hp { get; set; }
    }

    public class EventUpdate
    {
        public long Id { get; set; }
        public string Method { get; set; }
        public string Info { get; set; }
    }
    
    public class ItemUpdate
    {
        public long Id { get; set; }
        public string ModelInfo { get; set; }
        public int Count { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
    }
}

using UnityEngine;

namespace _Games.Scripts.Netwotking
{
    public class GameManager
    {
        public long UnitId { get; set; }
        public string GameModeId { get; set; }
        //public Unit SelectedUnit { get; set; }
        public GameObject MenuCharacter { get; set; }

        private static GameManager manager;

        public static GameManager GetGameManager()
        {
            return manager = manager ?? new GameManager();
        }
    }
}
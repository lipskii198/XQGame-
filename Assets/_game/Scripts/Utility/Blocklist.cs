using System.Collections.Generic;
using UnityEngine;

namespace _game.Scripts.Utility
{
    public class Blocklist
    {
        private readonly HashSet<object> blockers = new();
        
        public void RegisterBlocker(object blocker)
        {
            blockers.Add(blocker);
        }
        
        public void UnregisterBlocker(object blocker)
        {
            blockers.Remove(blocker);
        }
        
        public bool IsBlocked()
        {
            return blockers.Count > 0;
        }
    }
}
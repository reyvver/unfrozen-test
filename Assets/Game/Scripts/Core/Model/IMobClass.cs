using System;

namespace Game.Scripts.Core.Model
{
    public interface IMobClass : ISelectable, ILiveObject
    {
        public event Action StateChanged;

        public enum State
        {
            Idle,
            Damaged,
            DoDamage
        }
        
        public State CurrentState { set; }
        public void RestoreHp(int startHp);
    }
}
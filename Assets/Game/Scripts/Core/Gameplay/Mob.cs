using UnityEngine;

namespace Game.Scripts.Core.Gameplay
{
    public abstract class Mob : MonoBehaviour, ISelectable, IDamageable
    {
        // если спавнить мобов из кода на сцену
        // то тогда можно не нарушать инкапсуляцию
        public enum MobOwner
        {
            Player,
            Enemy
        }
        [HideInInspector] public MobOwner owner;
        
        public virtual void Select()
        {
        }

        public virtual void Deselect()
        {
        }

        public virtual float GetDamage()
        {
            return 0;
        }

        public virtual void DoDamage()
        {
            
        }

        public virtual void WasDamagedBy(float damage)
        {
            
        }
    }
}
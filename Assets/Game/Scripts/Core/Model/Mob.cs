using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Core.Model
{
    public abstract class Mob : MonoBehaviour, IDamageable
    {
        public enum MobOwner
        {
            Player,
            Enemy
        }

        public enum MobType
        {
            MinerRegular = 1,
            MinerElite = 2
        }
        
        public MobOwner Owner { get; private set; }
        public bool Alive { get; private set; }
        public bool ActionIsFinished { get; private set; }
        public float StartPosX { get; private set; }
        protected MobType MobClass { get; private set; }
        
        private bool isOnFight;
        private bool damageTaken;
        private IMobClass mobClass;

        public virtual void Init(MobOwner mobOwner, int order, MobType mobType)
        {
            Alive = true;
            Owner = mobOwner;
            StartPosX = transform.position.x;
            MobClass = mobType;
            UpdateChildren();
        }

        private void UpdateChildren()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).gameObject.layer =
                    LayerMask.NameToLayer(Owner == MobOwner.Player ? "Player" : "Enemy");
            }
        }
        
        public virtual float GetDamage()
        {
            return 0;
        }

        public void DoAttack()
        {
            if (mobClass == null) return;
            mobClass.CurrentState = IMobClass.State.DoDamage;
        }

        public void WasAttacked(float damage)
        {
            if (mobClass == null || !isOnFight || damageTaken) return;
            damageTaken = true;
            mobClass.CurrentState = IMobClass.State.Damaged;
            mobClass.ChangeHp(-damage);
        }

        protected void SetMobClass(IMobClass mob)
        {
            mobClass = mob;
            mobClass.StateChanged += AfterFight;
        }

        public void ShowSelection(bool value)
        {
            if (value) mobClass.Select();
            else mobClass.Deselect();
        }
        
        public virtual void PrepareForFight()
        {
            isOnFight = true;
            damageTaken = false;
            ActionIsFinished = false;
            mobClass.Deselect();
        }

        protected virtual void AfterFight()
        {
            ActionIsFinished = true;
            isOnFight = false;

            if (mobClass.Hp <= 0)
            {
                Alive = false;
            }
        }

        public virtual void ResetMob()
        {
            Alive = true;
        }

        public void MoveX(float newPosX, TweenCallback action = null)
        {
            transform.DOMoveX(newPosX, 1f).OnComplete(action);
        }
    }
}
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
        public MobOwner Owner { get; private set; }
        public bool Alive { get; private set; }
        public bool ActionIsFinished { get; private set; }
        public float StartPosX { get; private set; }

        private bool isOnFight;
        private IMobClass mobClass;
        protected int orderInLayer;

        public void Init(MobOwner mobOwner, int order)
        {
            Alive = true;
            Owner = mobOwner;
            StartPosX = transform.position.x;
            orderInLayer = order;
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
            if (mobClass == null || !isOnFight) return;
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
            if (value)
                mobClass.Select();
            else mobClass.Deselect();
        }
        
        public virtual void PrepareForFight()
        {
            isOnFight = true;
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
        
        public void MoveX(float newPosX, TweenCallback action = null)
        {
            transform.DOMoveX(newPosX, 1f).OnComplete(action);
        }
    }
}
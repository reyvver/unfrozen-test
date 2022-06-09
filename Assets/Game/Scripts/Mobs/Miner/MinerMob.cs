using System;
using Game.Scripts.Core;
using Spine.Unity;
using Spine.Unity.Modules;
using UnityEngine;

namespace Game.Scripts.Mobs.Miner
{
    public class MinerMob : MonoBehaviour, IDamageable, ISelectable
    {
        [SerializeField] private SkeletonAnimation animation;
        [SerializeField] private SkeletonGhost ghost;
        [SerializeField] private CollisionBodyPartDetection chest;
        [SerializeField] private MinerClass.MinerTypeDamage minerType;
        
        private MinerClass thisMob;
        
        private void Awake()
        {
            thisMob = new MinerClass(animation, ghost,minerType);
            chest.Collision += OnCollisionDetected;
        }
        
        private void OnCollisionDetected(IDamageable damageable)
        {
            if (thisMob.isDoingDamage) return;
            WasDamagedBy(damageable.GetDamage());
        }

        public float GetDamage()
        {
            return (int) minerType;  // разный дамаг в зависимости от типа шахтера
        }

        public void DoDamage()
        {
            thisMob.CurrentState = Mob.State.DoDamage;
        }

        public void WasDamagedBy(float damage)
        {
            thisMob.CurrentState = Mob.State.Damaged;
            thisMob.ChangeHp(-damage);
        }

        public void Select()
        {
            thisMob.Select();
        }

        public void Deselect()
        {
            thisMob.Deselect();
        }
    }
}
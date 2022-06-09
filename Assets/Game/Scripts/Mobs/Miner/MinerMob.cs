using Game.Scripts.Core.Gameplay;
using Spine.Unity;
using Spine.Unity.Modules;
using UnityEngine;

namespace Game.Scripts.Mobs.Miner
{
    public class MinerMob : Mob
    {
        [SerializeField] private SkeletonAnimation animation;
        [SerializeField] private SkeletonGhost ghost;
        [SerializeField] private CollisionBodyPartDetection chest;
        [SerializeField] private MinerClass.MinerTypeDamage minerType;
        
        private MinerClass thisMob;

        private void Awake()
        {
            thisMob = new MinerClass(animation, ghost, minerType);
            chest.Collision += OnCollisionDetected;
        }

        public override void Select()
        {
            thisMob.Select();
        }

        public override void Deselect()
        {
            thisMob.Deselect();
        }

        public override float GetDamage()
        {
            return (int) minerType;
        }

        public override void DoDamage()
        {
            thisMob.CurrentState = MobClass.State.DoDamage;
        }

        public override void WasDamagedBy(float damage)
        {
            thisMob.CurrentState = MobClass.State.Damaged;
            thisMob.ChangeHp(-damage);
        }
        
        private void OnCollisionDetected(IDamageable damageable)
        {
            if (thisMob.isDoingDamage) return;
            WasDamagedBy(damageable.GetDamage());
        }
    }
}
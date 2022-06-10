using Game.Scripts.Core.Model;
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
            SetMobClass(thisMob);
            chest.Collision += OnCollisionDetected;
        }
        
        public override float GetDamage()
        {
            return (int) minerType;
        }

        public override void PrepareForFight()
        {
            base.PrepareForFight();
            thisMob.ChangeLayer(250);
        }

        protected override void AfterFight()
        {
            base.AfterFight();
            thisMob.ChangeLayer(orderInLayer);
            
            if (!Alive)
                thisMob.HideOrShow(false);
        }

        private void OnCollisionDetected(IDamageable damageable)
        {
            if (thisMob.IsDoingDamage) return;
            WasAttacked(damageable.GetDamage());
        }
    }
}
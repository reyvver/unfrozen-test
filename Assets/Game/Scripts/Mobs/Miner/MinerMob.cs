using Game.Scripts.Core.Model;
using Spine.Unity;
using Spine.Unity.Modules;
using UnityEngine;

namespace Game.Scripts.Mobs.Miner
{
    public class MinerMob : Mob
    {
        [SerializeField] private new SkeletonAnimation animation;
        [SerializeField] private SkeletonGhost ghost;
        [SerializeField] private CollisionBodyPartDetection chest;
        
        private MinerClass thisMob;
        private MobType minerType;

        public override void Init(MobOwner mobOwner, int order, MobType mobType)
        {
            base.Init(mobOwner, order, mobType);
            minerType = mobType;
            thisMob = new MinerClass(animation, order, ghost, mobType);
            thisMob.ChangeSkin(mobType == MobType.MinerRegular ? "base" : "elite");
            SetMobClass(thisMob);
            chest.Collision += OnCollisionDetected;
        }

        public override float GetDamage()
        {
            return (int) MobClass;
        }

        public override void PrepareForFight()
        {
            base.PrepareForFight();
            thisMob.ChangeLayer("SelectedLiveObjects");
        }

        protected override void AfterFight()
        {
            base.AfterFight();
            thisMob.Deselect();
            
            if (!Alive)
                thisMob.HideOrShow(false);
        }

        public override void ResetMob()
        {
            base.ResetMob();
            thisMob.ChangeSkin(minerType == MobType.MinerRegular ? "base" : "elite");
            thisMob.HideOrShow(true);
            thisMob.RestoreHp((int)MobClass);
        }

        private void OnCollisionDetected(IDamageable damageable)
        {
            if (thisMob.IsDoingDamage) return;
            WasAttacked(damageable.GetDamage());
        }
    }
}
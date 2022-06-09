using System.Collections.Generic;
using Spine.Unity;
using UnityEngine;

namespace Game.Scripts.Mobs.Miner
{
    public class MinerClass : Mob
    {
        private enum Anim
        {
            Hit1,
            Hit2,
            Idle,
            Damaged
        }

        private readonly Dictionary<Anim, string> animNames = new Dictionary<Anim, string>
        {
            {Anim.Idle, "Idle"},
            {Anim.Damaged, "Damage"},
            {Anim.Hit1, "Miner_1"},
            {Anim.Hit2, "PickaxeCharge"},
        };

        public enum MinerTypeDamage
        {
            Normal = 1,
            Elite = 2
        }
        
        public MinerClass(SkeletonAnimation skeletonAnimation, MinerTypeDamage minerType) : base(skeletonAnimation)
        {
            Hp = (int) minerType * 2;
        }

        protected override void OnStateChanged(State newState)
        {
            base.OnStateChanged(newState);
            isDoingDamage = false;

            switch (newState)
            {
                case State.Idle:
                    SetAnimation(0, GetAnimName(Anim.Idle), true);
                    break;
                    
                case State.Damaged:
                    SetBloodSkin();
                    SetAnimation(0, GetAnimName(Anim.Damaged), false);
                    break;
                
                case State.DoDamage:
                    isDoingDamage = true;
                    Anim anim = SelectRandomHit();
                    SetAnimation(0, GetAnimName(anim), false);
                    break;
            }
            
        }

        private Anim SelectRandomHit()
        {
            int randomIndex = Random.Range(0, 2);
            return (Anim) randomIndex;
        }

        private string GetAnimName(Anim anim)
        {
            return animNames[anim];
        }
    }
}
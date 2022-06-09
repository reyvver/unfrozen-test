using Spine.Unity;
using UnityEngine;

namespace Game.Scripts.Mobs.Miner
{
    public class MinerMob : MonoBehaviour, IDamageable
    {
        [SerializeField] private SkeletonAnimation animation;
        [SerializeField] private CollisionBodyPartDetection chest;
        [SerializeField] private MinerClass.MinerTypeDamage minerType;
        
        private MinerClass thisMob;
        
        private void Awake()
        {
            thisMob = new MinerClass(animation, minerType);
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
    }
}
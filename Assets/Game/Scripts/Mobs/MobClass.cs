using Game.Scripts.Core.Gameplay;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;

namespace Game.Scripts.Mobs
{
    public class MobClass : ISelectable, ILiveObject
    {
        public enum State
        {
            Idle,
            Damaged,
            DoDamage
        }
        
        private State currentState;
        public State CurrentState
        {
            set => OnStateChanged(value);
        }
        public float Hp { get; set; }
        public bool isDoingDamage;
        
        private readonly SkeletonAnimation skeletonAnimation;
        private Skin bloodSkin;
        
        protected MobClass(SkeletonAnimation skeletonAnimation)
        {
            this.skeletonAnimation = skeletonAnimation;
            this.skeletonAnimation.Initialize(false);
            this.skeletonAnimation.state.Complete += AnimationCompleted;
            CurrentState = State.Idle;
        }
        
        private void AnimationCompleted(TrackEntry entry)
        {
            if (currentState != State.Idle)
                CurrentState = State.Idle;
        }
        
        protected virtual void OnStateChanged(State newState)
        {
            if (skeletonAnimation.state == null) return;
            currentState = newState;
        }
        
        protected void SetAnimation(int index, string animName, bool loop)
        {
            skeletonAnimation.state.SetAnimation(index, animName, loop);
        }
        
        protected void SetBloodSkin()
        {
            if (bloodSkin == null)
            {
                bloodSkin = skeletonAnimation.skeleton.Data.FindSkin("blood");
                if (bloodSkin == null)
                    return;
            }
        
            var newSkin = skeletonAnimation.skeleton.Skin;
            newSkin.AddAttachments(bloodSkin);
            skeletonAnimation.skeleton.SetSkin(newSkin);
            RefreshSkeletonAttachments();
        }
        
        private void RefreshSkeletonAttachments () {
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); 
        }
        
        public void ChangeHp(float value)
        {
            Hp += value;
        }
        
        public virtual void Select()
        {
        }
        
        public virtual void Deselect()
        {
        }
    }
}
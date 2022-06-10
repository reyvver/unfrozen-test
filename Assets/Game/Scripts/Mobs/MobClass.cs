using System;
using Game.Scripts.Core.Model;
using Spine;
using Spine.Unity;
using Spine.Unity.Modules.AttachmentTools;
using UnityEngine;
using State = Game.Scripts.Core.Model.IMobClass.State;

namespace Game.Scripts.Mobs
{
    public class MobClass : IMobClass
    {
        private State currentState;
        public event Action StateChanged;

        public State CurrentState
        {
            set => OnStateChanged(value);
        }

        public bool IsDoingDamage { get; protected set; }
        public float Hp { get; protected set; }
        
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
            {
                CurrentState = State.Idle;
                StateChanged?.Invoke();
            }
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
        
        public void HideOrShow(bool shown)
        {
            skeletonAnimation.gameObject.SetActive(shown);
        }
        
        public void ChangeLayer(int i)
        {
           skeletonAnimation.transform.GetComponent<MeshRenderer>().sortingOrder = i;
        }
    }
}
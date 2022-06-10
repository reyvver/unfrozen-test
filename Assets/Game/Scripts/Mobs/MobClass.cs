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
        private MeshRenderer meshRenderer;
        private Skin bloodSkin;
        
        protected MobClass(SkeletonAnimation skeletonAnimation, int orderInLayer)
        {
            this.skeletonAnimation = skeletonAnimation;
            this.skeletonAnimation.Initialize(false);
            this.skeletonAnimation.state.Complete += AnimationCompleted;
            meshRenderer = skeletonAnimation.transform.GetComponent<MeshRenderer>();
            CurrentState = State.Idle;
            meshRenderer.sortingOrder = orderInLayer;
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
            newSkin.Append(bloodSkin);
            skeletonAnimation.skeleton.SetSkin(newSkin);
            RefreshSkeletonAttachments();
        }
        
        private void RefreshSkeletonAttachments () {
            skeletonAnimation.Skeleton.SetSlotsToSetupPose();
            skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); 
            skeletonAnimation.LateUpdate();
        }
        
        public void ChangeHp(float value)
        {
            Hp += value;
        }
        
        public virtual void Select()
        {
            ChangeLayer("SelectedLiveObjects");
        }
        
        public virtual void Deselect()
        {
            ChangeLayer("LiveObjects");
        }
        
        public void HideOrShow(bool shown)
        {
            skeletonAnimation.gameObject.SetActive(shown);
        }
        
        public void ChangeLayer(string name)
        {
            meshRenderer.sortingLayerName = name;
        }

        public void ChangeSkin(string name)
        {
            if(skeletonAnimation.skeleton.Skin != null)
                skeletonAnimation.skeleton.Skin.Clear();
            
            var newSkin = skeletonAnimation.skeleton.Data.FindSkin(name).GetClone();

            skeletonAnimation.skeleton.SetSkin(newSkin);
            RefreshSkeletonAttachments();
        }
        
        public virtual void RestoreHp(int startHp)
        {
            Hp = startHp;
        }
    }
}
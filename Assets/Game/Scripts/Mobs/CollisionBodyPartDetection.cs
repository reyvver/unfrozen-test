using System;
using UnityEngine;

namespace Game.Scripts.Mobs
{
    public class CollisionBodyPartDetection : MonoBehaviour
    {
        public event Action<IDamageable> Collision;
        private void OnCollisionEnter2D(Collision2D col)
        {
            if (col.transform.parent.TryGetComponent(out IDamageable obj))
            {
                Collision?.Invoke(obj);
            }
        }
    }
}
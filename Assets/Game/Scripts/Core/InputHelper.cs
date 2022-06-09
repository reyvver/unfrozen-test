using System;
using Game.Scripts.Core.Gameplay;
using UnityEngine;

namespace Game.Scripts.Core
{
    public static class InputHelper
    {
        public static bool IsMobClicked(out Mob clickedGameObject)
        {
            if (!Input.GetMouseButtonDown(0))
            {
                clickedGameObject = null;
                return false;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
     
            RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray, Single.MaxValue, 1 << 8);

            if (hit2D.collider != null)
            {
                if (hit2D.transform.TryGetComponent(out Mob mob))
                {
                    clickedGameObject = mob;
                    return true;
                }
            } 
            
            clickedGameObject = null;
            return false;
        }
    }
}
using System;
using UnityEngine;

namespace Game.Scripts.Core
{
    public static class InputHelper
    {
        public static bool IsClicked(out ISelectable clickedGameObject)
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
                if (hit2D.transform.TryGetComponent(out ISelectable liveObject))
                {
                    clickedGameObject = liveObject;
                    return true;
                }
            } 
            
            clickedGameObject = null;
            return false;
        }
    }
}
using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Game
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (InputHelper.IsClicked(out ISelectable gameObject))
            {
                gameObject.Select();
            }
        }
    }
}
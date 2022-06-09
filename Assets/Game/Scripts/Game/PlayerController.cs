using Game.Scripts.Core;
using Game.Scripts.Core.Gameplay;
using UnityEngine;

namespace Game.Scripts.Game
{
    public class PlayerController : MonoBehaviour
    {
        private void Update()
        {
            if (InputHelper.IsMobClicked(out Mob mob))
            {
                mob.Select();
            }
        }
    }
}
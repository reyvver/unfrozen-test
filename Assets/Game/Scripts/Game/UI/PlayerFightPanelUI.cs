using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI
{
    public class PlayerFightPanelUI : Panel
    {
        [SerializeField] private Button attack;
        [SerializeField] private Button skip;

        public event Action<bool> Fight;
        
        public void Init()
        {
            attack.onClick.AddListener(() =>
            {
                panel.interactable = false;
                Fight?.Invoke(true);
            });
            
            skip.onClick.AddListener(() =>
            {
                panel.interactable = false;
                Fight?.Invoke(false);
            });
        }
    }
}
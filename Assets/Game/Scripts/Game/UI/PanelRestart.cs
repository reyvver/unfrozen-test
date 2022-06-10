using System;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI
{
    public class PanelRestart : Panel
    {
        [SerializeField] private Button restartButton;

        public event Action Restart;

        private void Awake()
        {
            restartButton.onClick.AddListener(() =>
            {
                Restart?.Invoke();
            });
        }
    }
}
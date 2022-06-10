using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI
{
    public class PlayerFightPanelUI : MonoBehaviour
    {
        [SerializeField] private Button attack;
        [SerializeField] private Button skip;

        public event Action<bool> Fight;
        private CanvasGroup panel;
        
        private void Awake()
        {
            panel = GetComponent<CanvasGroup>();
            
            attack.onClick.AddListener(() =>
            {
                Fight?.Invoke(true);
            });
            
            skip.onClick.AddListener(() =>
            {
                Fight?.Invoke(false);
            });
        }

        public void ShowPanel(bool value)
        {
            panel.DOFade(value ? 1 : 0, 0.35f);
        }
    }
}
using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.Game.UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class Panel : MonoBehaviour
    {
        protected CanvasGroup panel;

        public void ShowPanel(bool isShown)
        {
            if (panel == null) panel = GetComponent<CanvasGroup>();;
            panel.DOFade(isShown ? 1 : 0, 0.35f).OnComplete(() => panel.interactable = isShown);
        }
    }
}
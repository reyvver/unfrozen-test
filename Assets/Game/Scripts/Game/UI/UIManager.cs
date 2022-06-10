using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text turnIndicator;
        [SerializeField] private Image blackout;
        public PlayerFightPanelUI panelFight;

        public void SetText(GameplayManager.GameState state)
        {
            switch (state)
            {
                case GameplayManager.GameState.PlayerTurn:
                    turnIndicator.text = "Player turn"; 
                    break;
                
                case GameplayManager.GameState.EnemyTurn:
                    turnIndicator.text = "Enemy turn"; 
                    break;
                
                case GameplayManager.GameState.GameLose:
                    turnIndicator.text = "Game lose"; 
                    break;
                
                case GameplayManager.GameState.GameWin:
                    turnIndicator.text = "Game win"; 
                    break;
                
                case GameplayManager.GameState.ChooseOption:
                    turnIndicator.text = "Choose fight or skip"; 
                    break;
                
                case GameplayManager.GameState.ChooseEnemy:
                    turnIndicator.text = "Click on enemy"; 
                    break;
            }
        }

        public void DoBlackoutFade(bool fadeIn)
        {
            blackout.DOFade(fadeIn ? 0.6f : 0, 1f);
        }
    }
}
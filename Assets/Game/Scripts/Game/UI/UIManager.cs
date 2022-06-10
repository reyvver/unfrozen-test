using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Text turnIndicator;
        [SerializeField] private Image blackout;
        [SerializeField] private PanelRestart panelRestart;
        [SerializeField] private PlayerFightPanelUI panelFight;

        public PlayerFightPanelUI PanelFight => panelFight;
        public PanelRestart PanelRestart => panelRestart;

        public void Init()
        {
            panelFight.Init();
        }
        
        public void SetText(GameplayManager.GameState state)
        {
            switch (state)
            {
                case GameplayManager.GameState.PlayerTurn:
                    turnIndicator.text = "Player turn: Choose fight or skip"; 
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

                case GameplayManager.GameState.ChooseEnemy:
                    turnIndicator.text = "Click on enemy"; 
                    break;
            }
        }

        public void DoBlackoutFade(bool shown)
        {
            blackout.DOFade(shown ? 0.6f : 0, 1f);
        }

        public void DoFightPanelFade(bool shown)
        {
            panelFight.ShowPanel(shown);
        }
        
    }
}
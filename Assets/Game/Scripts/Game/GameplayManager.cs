using Game.Scripts.Game.UI;
using UnityEngine;

namespace Game.Scripts.Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private UIManager ui;
        [SerializeField] private BattleManager battle;

        public enum GameState
        {
            PlayerTurn,
            EnemyTurn,
            GameWin,
            GameLose,
            ChooseEnemy
        }

        private GameState State
        {
            set => UpdateState(value);
        }

        private void Awake()
        {
            battle.Init();
            battle.DoFade += HideFade;
            battle.GameFinished += OnGameFinished;
            battle.FightStatus += OnFightStatusChanged;

            ui.Init();
            ui.PanelRestart.Restart += RestartGame;
            ui.PanelFight.Fight += OnPlayerChoice;

            StartGame();
        }

        private void StartGame()
        {
            battle.SetBattleQueue();
            HandleChangeTurn();
        }

        private void UpdateState(GameState value)
        {
            ui.SetText(value);
            ui.DoFightPanelFade(battle.PlayerTurn);
        }

        private void OnPlayerChoice(bool isFight)
        {
            if (isFight) UpdateState(GameState.ChooseEnemy);
            battle.OnPlayerTurn(isFight);
        }

        private void OnGameFinished(bool result)
        {
            State = result ? GameState.GameWin : GameState.GameLose;
            ui.PanelRestart.ShowPanel(true);
            ui.PanelFight.ShowPanel(false);
        }

        private void OnFightStatusChanged(bool status)
        {
            if (status) HandleChangeTurn();
            else
            {
                ui.DoFightPanelFade(false);
                ui.DoBlackoutFade(true);
            }
        }

        private void HideFade()
        {
            ui.DoBlackoutFade(false);
        }

        private void HandleChangeTurn()
        {
            if (!battle.BattleFinished)
            {
                battle.ChangeTurn();
                State = battle.PlayerTurn ? GameState.PlayerTurn : GameState.EnemyTurn;
            }
        }

        private void RestartGame()
        {
            ui.PanelRestart.ShowPanel(false);
            battle.ResetBattle();
            HandleChangeTurn();
        }
    }
}
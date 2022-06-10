using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Core;
using Game.Scripts.Core.Model;
using Game.Scripts.Game.UI;
using UnityEngine;

namespace Game.Scripts.Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private Transform FightPosLeft;
        [SerializeField] private Transform FighTPosRight;

        [SerializeField] private UIManager ui;
        
        [SerializeField] private GameObject mobPrefab;
        [SerializeField] private List<Transform> playerSquad;
        [SerializeField] private List<Transform> enemySquad;
        
        private readonly MobsManager mobsManager = new MobsManager();
        private Coroutine waiter;
        private bool playerTurn;
        private bool chooseToFight;
        private Mob currentMobTurn;
        private Mob attackedMob;

        public enum GameState
        {
            PlayerTurn,
            EnemyTurn,
            GameWin,
            GameLose,
            ChooseOption,
            ChooseEnemy
        }

        public GameState State
        {
            set => UpdateState(value);
        }

        private void Awake()
        {
           mobsManager.SpawnMobs(playerSquad, Mob.MobOwner.Player, mobPrefab);
           mobsManager.SpawnMobs(enemySquad, Mob.MobOwner.Enemy, mobPrefab);
           mobsManager.SetMobsQueue();

           ui.panelFight.Fight += PlayerChoice;
           ChangeTurn();
        }

        private void ChangeTurn()
        {
            if (CheckGameEnd())
            {
                return;
            }
            
            if (waiter != null)
                StopCoroutine(waiter);
            
            currentMobTurn = mobsManager.GetMob(out playerTurn);
            currentMobTurn.ShowSelection(true);
            chooseToFight = false;
            ui.panelFight.ShowPanel(playerTurn);
            UpdateState(playerTurn ? GameState.PlayerTurn : GameState.EnemyTurn);
            if (!playerTurn) OnEnemyTurn();
        }

        private void OnEnemyTurn()
        {
            attackedMob = mobsManager.GetRandomMob(Mob.MobOwner.Player);
            ManageAttack(attackedMob);
        }

        private void Update()
        {
            if (!playerTurn ) return;

            if (InputHelper.IsMobClicked(out Mob clickedMob))
            {
                if (chooseToFight)
                {
                    if (clickedMob.Owner == Mob.MobOwner.Enemy && clickedMob.Alive)
                    {
                        attackedMob = clickedMob;
                        ManageAttack(attackedMob);
                    }
                }
                else 
                    UpdateState(GameState.ChooseOption);
            }
        }

        private void ManageAttack(Mob attacked)
        {
            ui.DoBlackoutFade(true);
            
            currentMobTurn.PrepareForFight();
            currentMobTurn.MoveX(currentMobTurn.Owner == Mob.MobOwner.Player? FightPosLeft.position.x : FighTPosRight.position.x, StartAttack);
            
            attacked.PrepareForFight();
            attacked.MoveX(attacked.Owner == Mob.MobOwner.Player? FightPosLeft.position.x : FighTPosRight.position.x);
        }

        private void StartAttack()
        {
            currentMobTurn.DoAttack();
            waiter = StartCoroutine(WaitForFighters());
        }


        IEnumerator WaitForFighters()
        {
            yield return new WaitUntil(() => currentMobTurn.ActionIsFinished && attackedMob.ActionIsFinished);

            if (attackedMob.Alive)
            {
                mobsManager.ReturnMob(attackedMob);
            }
            else mobsManager.RemoveDeadMobFromQueue();
            
            ui.DoBlackoutFade(false);
            mobsManager.ReturnMob(currentMobTurn);
            currentMobTurn.MoveX(currentMobTurn.StartPosX);
            attackedMob.MoveX(attackedMob.StartPosX, ChangeTurn);
        }

        private bool CheckGameEnd()
        {
            if (playerTurn && !mobsManager.HasEnemyMobs())
            {
                UpdateState(GameState.GameWin);
                return true;
            }

            if (!playerTurn && !mobsManager.HasPlayerMobs())
            {
                UpdateState(GameState.GameLose);
                return true;
            }

            return false;
        }
        
        private void UpdateState(GameState value)
        {
           ui.SetText(value);
        }

        private void PlayerChoice(bool value)
        {
            chooseToFight = value;
            if (!chooseToFight) Skip();
                else UpdateState(GameState.ChooseEnemy);
        }

        private void Skip()
        {
            mobsManager.ReturnMob(currentMobTurn);
            currentMobTurn.ShowSelection(false);
            ChangeTurn();
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Game.Scripts.Core;
using Game.Scripts.Core.Model;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Game
{
    public class BattleManager : MonoBehaviour
    {
        [SerializeField] private Transform fightPosLeft;
        [SerializeField] private Transform fightPosRight;
        
        [SerializeField] private GameObject mobPrefab;
        [SerializeField] private List<Transform> playerSquad;
        [SerializeField] private List<Transform> enemySquad;

        public bool PlayerTurn => playerTurn;
        public bool BattleFinished => CheckGameEnd();
        
        public event Action<bool> GameFinished;
        public event Action<bool> FightStatus;
        public event Action DoFade;
        
        private readonly MobsManager mobsManager = new MobsManager();
        private const float EnemyChanceToSkip = 55;
        
        private Coroutine waiter;
        private bool playerTurn;
        private bool chooseToFight;
        private Mob currentMobTurn;
        private Mob attackedMob;
        
        public void Init()
        {
           mobsManager.SpawnMobs(playerSquad.GetRange(0,2), Mob.MobOwner.Player, Mob.MobType.MinerRegular, mobPrefab);
           mobsManager.SpawnMobs(playerSquad.GetRange(2,2), Mob.MobOwner.Player, Mob.MobType.MinerElite, mobPrefab);
           mobsManager.SpawnMobs(enemySquad.GetRange(0,2), Mob.MobOwner.Enemy, Mob.MobType.MinerRegular, mobPrefab);
           mobsManager.SpawnMobs(enemySquad.GetRange(2,2), Mob.MobOwner.Enemy, Mob.MobType.MinerElite, mobPrefab);
        }

        public void SetBattleQueue()
        {
            mobsManager.SetMobsQueue();
        }

        public void ResetBattle()
        {
            mobsManager.ResetAll();
            mobsManager.SetMobsQueue();
        }

        public void ChangeTurn()
        {
            if (waiter != null) StopCoroutine(waiter);
            
            currentMobTurn = mobsManager.GetMob(out playerTurn);
            currentMobTurn.ShowSelection(true);
            
            chooseToFight = false;
            if (!playerTurn) OnEnemyTurn();
        }

        public void OnPlayerTurn(bool isFight)
        {
            chooseToFight = isFight;
            if (!isFight) Skip();
        }

        private void OnEnemyTurn()
        {
            float chance = Random.Range(0f, 1f);
            bool attack = chance < EnemyChanceToSkip / 100f;
            StartCoroutine(WaitForEnemyAction(attack));
        }

        private void Update()
        {
            if (!playerTurn) return;

            if (InputHelper.IsMobClicked(out Mob clickedMob))
            {
                if (chooseToFight)
                {
                    if (clickedMob.Owner == Mob.MobOwner.Enemy && clickedMob.Alive)
                    {
                        attackedMob = clickedMob;
                        ManageAttack();
                    }
                }
            }
        }

        private void ManageAttack()
        {
            FightStatus?.Invoke(false);
            
            currentMobTurn.PrepareForFight();
            currentMobTurn.MoveX(currentMobTurn.Owner == Mob.MobOwner.Player? fightPosLeft.position.x : fightPosRight.position.x, StartAttack);
            
            attackedMob.PrepareForFight();
            attackedMob.MoveX(attackedMob.Owner == Mob.MobOwner.Player? fightPosLeft.position.x : fightPosRight.position.x);
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
            
            DoFade?.Invoke();
            
            mobsManager.ReturnMob(currentMobTurn);
            currentMobTurn.MoveX(currentMobTurn.StartPosX, MobPlaced);
            attackedMob.MoveX(attackedMob.StartPosX);
        }

        IEnumerator WaitForEnemyAction(bool attack)
        {
            yield return new WaitForSeconds(1.5f);
            if (attack)
            {
                attackedMob = mobsManager.GetRandomMob(Mob.MobOwner.Player);
                ManageAttack();
            }
            else Skip();
        }

        private bool CheckGameEnd()
        {
            if (playerTurn && !mobsManager.HasEnemyMobs())
            {
                GameFinished?.Invoke(true);
                return true;
            }

            if (!playerTurn && !mobsManager.HasPlayerMobs())
            {
                GameFinished?.Invoke(false);
                return true;
            }

            return false;
        }
        
        private void Skip()
        {
            mobsManager.ReturnMob(currentMobTurn);
            currentMobTurn.ShowSelection(false);
            currentMobTurn = null;
            FightStatus?.Invoke(true);
        }

        private void MobPlaced()
        {
            currentMobTurn = null;
            FightStatus?.Invoke(true);
        }
        
    }
}
using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Core;
using Game.Scripts.Core.Gameplay;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private List<Mob> playerMobs;
        [SerializeField] private List<Mob> enemyMobs;

        private readonly Queue<Mob> mobsQueue = new Queue<Mob>();
        private bool playerTurn;
        
        private void Start()
        {
            SetMobsQueue();
            StartGame();
        }

        private void SetMobsQueue()
        {
            foreach (var mob in playerMobs)
            {
                mob.owner = Mob.MobOwner.Player;
            }
            
            foreach (var mob in enemyMobs)
            {
                mob.owner = Mob.MobOwner.Enemy;
            }

            var allMobs = playerMobs.ToList();
            allMobs.AddRange(enemyMobs);
            List<int> indexChecked = new List<int>();
            int previousMob = -1;

            for (int i = 0; i < allMobs.Count; i++)
            {
                while (true)
                {
                    int random = Random.Range(0, allMobs.Count);

                    if (random != previousMob)
                    {
                        previousMob = random;

                        if (!indexChecked.Contains(random))
                        {
                            indexChecked.Add(random);
                            mobsQueue.Enqueue(allMobs[random]);
                        }
                        
                        break;
                    }
                }
            }
        }

        private void StartGame()
        {
            var mob = mobsQueue.Dequeue();
            mob.Select();
            playerTurn = mob.owner == Mob.MobOwner.Player;
            
            if (playerTurn)
                PlayerTurn();
            else 
                EnemyTurn();
        }

        private void Update()
        {
            if (!playerTurn) return;

            if (InputHelper.IsMobClicked(out Mob selectedMob))
            {
                if (selectedMob.owner == Mob.MobOwner.Enemy)
                {
                    DoDamageToMob();
                }
            }
        }

        private void EnemyTurn()
        {
            var mob = GetRandomPlayerMob();
            Debug.Log(mob.name);
        }

        private void PlayerTurn()
        {
            
        }

        private void DoDamageToMob()
        {
            
        }

        private Mob GetRandomPlayerMob()
        {
            int randomIndex = Random.Range(0, playerMobs.Count);
            return playerMobs[randomIndex];
        }
    }
}
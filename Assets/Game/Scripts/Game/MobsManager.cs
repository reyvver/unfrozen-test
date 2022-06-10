using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Core.Model;
using UnityEngine;

namespace Game.Scripts.Game
{
    public class MobsManager
    {
        private readonly Dictionary<Mob.MobOwner, List<Mob>> mobs = new Dictionary<Mob.MobOwner, List<Mob>>();
        private Queue<Mob> mobsQueue = new Queue<Mob>();

        public void SpawnMobs(List<Transform> places, Mob.MobOwner owner, GameObject prefab)
        {
            mobs.Add(owner, new List<Mob>());
            int orderInLayer = 0;
            
            foreach (var place in places)
            {
                var mob = CreateNewMob(prefab, place, owner);
                mob.Init(owner, orderInLayer);
                mobs[owner].Add(mob);
                orderInLayer++;
            }
        }

        private Mob CreateNewMob(GameObject prefab, Transform place, Mob.MobOwner owner)
        {
            var newMob = Object.Instantiate(prefab, place);
            var newMobScript = newMob.GetComponent<Mob>();
            return newMobScript;
        }

        public void SetMobsQueue()
        {
            var allMobs = mobs[Mob.MobOwner.Player].ToList();
            allMobs.AddRange(mobs[Mob.MobOwner.Enemy]);
            
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

        public Mob GetMob(out bool ownerIsPlayer)
        {
            var mob = mobsQueue.Dequeue();
            ownerIsPlayer = mob.Owner == Mob.MobOwner.Player;
            return mob;
        }

        public void ReturnMob(Mob mob)
        {
            mobsQueue.Enqueue(mob);
        }

        public Mob GetRandomMob(Mob.MobOwner owner)
        {
            return mobs[owner].Find(x => x.Alive);
        }

        public bool HasPlayerMobs()
        {
            return mobs[Mob.MobOwner.Player].Exists(x => x.Alive);
        }
        
        public bool HasEnemyMobs()
        {
            return mobs[Mob.MobOwner.Enemy].Exists(x => x.Alive);
        }

        public void RemoveDeadMobFromQueue()
        {
            mobsQueue = new Queue<Mob>(mobsQueue.Where(m => m.Alive));
        }
     
    }
}
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
        private int orderInLayer = 0;


        public void SpawnMobs(List<Transform> places, Mob.MobOwner owner, Mob.MobType mobType, GameObject prefab)
        {
            if (!mobs.ContainsKey(owner))
                mobs.Add(owner, new List<Mob>());
            
            foreach (var place in places)
            {
                var mob = CreateNewMob(prefab, place, owner);
                mob.Init(owner, orderInLayer, mobType);
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
            mobsQueue.Clear();

            var allMobs = mobs[Mob.MobOwner.Player].ToList();
            allMobs.AddRange(mobs[Mob.MobOwner.Enemy]);
            
            var rnd = new System.Random();
            var randomized = allMobs.OrderBy(item => rnd.Next());

            foreach (var mob in randomized)
            {
                mobsQueue.Enqueue(mob);
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

        public void ResetAll()
        {
            foreach (var mobList in mobs.Values)
            {
                foreach (var mob in mobList)
                {
                    mob.ResetMob();
                }
            }
        }
    }
}
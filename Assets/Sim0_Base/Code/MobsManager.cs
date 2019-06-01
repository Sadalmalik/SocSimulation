using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationBase
{
    public class MobsManager
    {
        private static MobsManager instance_;
        public static MobsManager instance
        {
            get
            {
                if (instance_ == null)
                    instance_ = new MobsManager();
                return instance_;
            }
        }

        public Mob prefab;

        public List<MobBehaviour> mobs { get; private set; }
        private List<MobBehaviour> newMobs_;

        private List<MobBehaviour> mobsToRemove_;
        private Dictionary<Type, Stack<MobBehaviour>> mobsPool_;

        private bool iterating_;

        private MobsManager()
        {
            iterating_ = false;
            mobs = new List<MobBehaviour>();
            newMobs_ = new List<MobBehaviour>();
            mobsToRemove_ = new List<MobBehaviour>();
            mobsPool_ = new Dictionary<Type, Stack<MobBehaviour>>();
        }

        public T CreateMob<T>() where T : MobBehaviour, new()
        {
            T mob=null;
            Stack<MobBehaviour> stack = null;
            if (mobsPool_.TryGetValue(typeof(T), out stack))
            {
                if (stack.Count>0)
                    mob = stack.Pop() as T;
            }
            else
            {
                stack = new Stack<MobBehaviour>();
                mobsPool_.Add(typeof(T), stack);
            }
            if (mob == null)
            {
                Mob model = GameObject.Instantiate<Mob>(prefab);
                mob = new T();
                mob.Init(model);
            }

            (iterating_ ? newMobs_ : mobs).Add(mob);

            mob.Reset();
            return mob;
        }

        public void Tick()
        {
            Stack<MobBehaviour> stack = null;

            iterating_ = true;
            foreach (var mob in mobs)
            {
                mob.Tick();
                if (!mob.alive)
                    mobsToRemove_.Add(mob);
            }
            mobs.AddRange(newMobs_);
            newMobs_.Clear();
            iterating_ = false;

            foreach (var mob in mobsToRemove_)
            {
                mob.Kill();
                mobs.Remove(mob);
                Type type = mob.GetType();
                if (!mobsPool_.TryGetValue(type, out stack))
                {
                    stack = new Stack<MobBehaviour>();
                    mobsPool_.Add(type, stack);
                }
                stack.Push(mob);
            }

            mobsToRemove_.Clear();
        }
    }
}
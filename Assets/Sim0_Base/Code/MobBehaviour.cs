using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationBase
{
    [Serializable]
    public class MobSettings
    {
        public float size=1;
        public float sense=1;
    }

    public abstract class MobBehaviour
    {
        public Mob mob;
        public MobSettings mobSettings;

        public bool alive;

        public virtual void Init(Mob mob)
        {
            this.mob = mob;
            this.alive = true;
            mob.behaviour = this;
            mob.name = "Mob:" + this.GetType().Name;
        }

        public void Init(MobSettings settings)
        {
            mobSettings = settings;
            mob.ApplyParams(settings.size, settings.sense);
        }

        public virtual void Reset()
        {
            alive = true;
            mob.Active = true;
        }

        public virtual void Kill()
        {
            alive = false;
            mob.Active = false;
        }

        public virtual void Tick()
        {

        }
    }
}

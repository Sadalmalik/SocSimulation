using SimulationBase;
using System;
using UnityEngine;

namespace Simulation2
{
    [Serializable]
    public class FisherManSettings
    {
        public float lifeTime = 100;
        public Color fishermanColor = new Color(.85f, .65f, .35f);
        public float hungerDelay = 10;
        public bool stockpile = false;
        public int hungerAmount = 3;
    }

    public class FisherMan : MoveRandomInCircle
    {
        public FisherManSettings fishermanSettings;
        
        private int food = 0;
        //private float deathTime_;
        private Delay deathDelay_ = new Delay();
        private Delay hungerDelay_ = new Delay();
        
        public override void Init(Mob mob)
        {
            base.Init(mob);

            mob.TriggerEnter += HandleTrigger;
        }

        public void Init(FisherManSettings settings)
        {
            fishermanSettings = settings;

            mob.SetColor(settings.fishermanColor);

            deathDelay_.Set(settings.lifeTime);
            hungerDelay_.Set(settings.hungerDelay);
        }

        public override void Reset()
        {
            base.Reset();

            deathDelay_.Start();
            hungerDelay_.Start();
        }

        public override void Tick()
        {
            base.Tick();

            if (!alive) return;

            if (deathDelay_.IsComplete())
                Kill();

            if (hungerDelay_.IsComplete())
            {
                if (food > 0)
                {
                    //  Съедаем всё
                    food = 0;// Mathf.Max(food - fishermanSettings.hungerAmount, 0);

                    hungerDelay_.Start();
                }
                else Kill();
            }
        }
        
        private void HandleTrigger(Collider other)
        {
            if (fishermanSettings.stockpile || food < fishermanSettings.hungerAmount)
            {
                Mob mob = other.GetComponent<Mob>();
                if (mob == null) return;
                Fish fish = mob.behaviour as Fish;
                if (fish == null) return;
                fish.Kill();
                food++;
            }
        }
    }
}

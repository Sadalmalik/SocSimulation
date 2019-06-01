using SimulationBase;
using System;
using UnityEngine;

namespace Simulation2
{
    [Serializable]
    public class FishSettings
    {
        public float lifeTime = 30;
        public Color fishColor = new Color(.35f, .85f, .65f);
        public float reproductionDelay = 5;
        public int reproductionAmount = 3;
    }

    public class Fish : MoveRandomInCircle
    {
        public FishSettings fishSettings;

        private Delay deathDelay_ = new Delay();
        private Delay reproduceDelay_ = new Delay();
        
        public override void Init(Mob mob)
        {
            base.Init(mob);

            mob.TriggerEnter += HandleTrigger;
        }

        public void Init(FishSettings settings)
        {
            fishSettings = settings;

            mob.SetColor(settings.fishColor);

            deathDelay_.Set(settings.lifeTime);
            reproduceDelay_.Set(settings.reproductionDelay);
        }

        public override void Reset()
        {
            base.Reset();

            deathDelay_.Start();
            reproduceDelay_.Start(0.35f);
        }

        public override void Tick()
        {
            base.Tick();

            if (deathDelay_.IsComplete())
                Kill();
        }
        
        private void HandleTrigger(Collider other)
        {
            if (reproduceDelay_.IsComplete())
            {
                Mob mob = other.GetComponent<Mob>();
                if (mob == null) return;
                Fish fish = mob.behaviour as Fish;
                if (fish != null && fish.reproduceDelay_.IsComplete())
                {
                    this.reproduceDelay_.Start();
                    fish.reproduceDelay_.Start();

                    for (int i = 0; i < fishSettings.reproductionAmount; i++)
                    {
                        Fish newFish = MobsManager.instance.CreateMob<Fish>();
                        Vector2 offset = UnityEngine.Random.insideUnitCircle;
                        newFish.mob.transform.position = mob.transform.position + new Vector3(offset.x, 0, offset.y);
                        newFish.Init(mobSettings);
                        newFish.Init(moveSettings);
                        newFish.Init(fishSettings);
                    }
                }
            }
        }
    }
}
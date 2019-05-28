using SimulationBase;
using System;
using UnityEngine;

namespace Simulation2
{
    public class Fish
    {
        public static Action<Vector3> CreateFish;

        public static float reproductionDelay = 10;
        public static float lifeTime = 50;
        public static int reproduceAmount = 2;


        public bool dead;
        private Mob mob_;
        private float deathTime_;
        private float reproduceTime_;

        public Mob Mob { get { return mob_; } }

        public Fish(Mob mob)
        {
            mob_ = mob;
            mob_.controller = this;
            mob_.TriggerStay += HandleTrigger;
            Born();
        }

        public void Born()
        {
            dead = false;
            deathTime_ = Time.time + lifeTime;
            mob_.Active = true;
            UpdateReproduction(.35f);
        }

        public void Kill()
        {
            dead = true;
            mob_.Active = false;
        }

        public void FixedUpdate()
        {
            if (deathTime_ < Time.time)
                Kill();
        }

        private bool ReadyToReproduction() { return reproduceTime_ < Time.time; }
        private void UpdateReproduction(float coef=1) { reproduceTime_ = Time.time + reproductionDelay * coef; }

        private void HandleTrigger(Collider other)
        {
            if (ReadyToReproduction())
            {
                Mob mob = other.GetComponent<Mob>();
                if (mob == null) return;
                Fish fish = mob.controller as Fish;
                if (fish != null && fish.ReadyToReproduction())
                {
                    this.UpdateReproduction();
                    fish.UpdateReproduction();

                    for (int i = 0; i < reproduceAmount; i++)
                        CreateFish(mob_.transform.position);
                }
            }
        }
    }
}
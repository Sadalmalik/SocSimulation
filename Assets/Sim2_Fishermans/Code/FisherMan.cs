using UnityEngine;
using SimulationBase;

namespace Simulation2
{
    public class FisherMan
    {
        public static float hungerDelay = 10;
        public static float lifeTime = 100;
        public static bool stockpile = false;
        public static int collectLimit = 3;



        public bool dead = false;
        private Mob mob_;
        private int food = 0;
        private float deathTime_;

        public FisherMan(Mob mob)
        {
            mob_ = mob;
            mob_.controller = this;
            mob_.TriggerStay += HandleTrigger;
            UpdateDeath();
            Born();
        }

        public void Born()
        {
            dead = false;
            deathTime_ = Time.time + lifeTime;
            mob_.Active = true;
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
            if (dead) return;

            if (ReadyToDeath())
            {
                if (food > 0)
                {
                    food--;
                    UpdateDeath();
                }
                else Kill();
            }
        }

        private bool ReadyToDeath() { return deathTime_ < Time.time; }
        private void UpdateDeath() { deathTime_ = Time.time + hungerDelay; }

        private void HandleTrigger(Collider other)
        {
            if (stockpile || food < collectLimit)
            {
                Mob mob = other.GetComponent<Mob>();
                if (mob == null) return;
                Fish fish = mob.controller as Fish;
                if (fish == null) return;
                fish.Kill();
                food++;
            }
        }
    }
}

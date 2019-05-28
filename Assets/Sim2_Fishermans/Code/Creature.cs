using UnityEngine;
using SimulationBase;

namespace Simulation2
{
    public class Creature
    {
        public bool alive;
        public float deathTime_;

        public Creature(Mob mob, float life)
        {

        }

        public bool IsDead()
        {
            return deathTime_ < Time.time;
        }
    }
}
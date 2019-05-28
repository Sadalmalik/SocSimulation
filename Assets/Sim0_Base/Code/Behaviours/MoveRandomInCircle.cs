using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationBase
{
    public class MoveRandomInCircle : MobBehaviour
    {
        public float speed = 1;
        public float range = 45;
        public float angle = 15;

        public override void FixedUpdate()
        {
            mob.transform.Translate(Vector3.forward * speed * 10 * Time.fixedDeltaTime);
            mob.transform.Rotate(Vector3.up, Random.Range(-angle, angle));

            if (range < mob.transform.position.magnitude)
                mob.transform.LookAt(Vector3.zero);
        }
    }
}

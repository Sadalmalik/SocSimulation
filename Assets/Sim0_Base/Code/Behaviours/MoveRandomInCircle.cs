using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationBase
{
    [Serializable]
    public class MoveRandomInCircleSettings
    {
        public static readonly MoveRandomInCircleSettings Default = new MoveRandomInCircleSettings();

        public float speed = 1;
        public float range = 45;
        public float angle = 15;
    }

    public class MoveRandomInCircle : MobBehaviour
    {
        public MoveRandomInCircleSettings moveSettings = null;

        public void Init(MoveRandomInCircleSettings settings)
        {
            moveSettings = settings;
        }

        public override void Tick()
        {
            if (moveSettings == null) moveSettings = MoveRandomInCircleSettings.Default;

            mob.transform.Translate(Vector3.forward * moveSettings.speed * 10 * Time.fixedDeltaTime);
            mob.transform.Rotate(Vector3.up, UnityEngine.Random.Range(-moveSettings.angle, moveSettings.angle));

            if (moveSettings.range < mob.transform.position.magnitude)
                mob.transform.LookAt(Vector3.zero);
        }
    }
}

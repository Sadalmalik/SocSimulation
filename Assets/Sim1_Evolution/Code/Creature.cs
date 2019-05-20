using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Simulation1
{
    [Serializable]
    public class Params
    {
        [Range(0.1f, 10)] public float speed = 1;
        [Range(0.1f, 10)] public float size = 1;
        [Range(0.1f, 10)] public float sense = 1;

        public void Mutate(float rate = 0.1f)
        {
            speed += Random.Range(-rate, rate);
            size += Random.Range(-rate, rate);
            sense += Random.Range(-rate, rate);
        }

        public Vector3 ToVector3()
        {
            return new Vector3(speed, size, sense);
        }
    }

    [ExecuteInEditMode]
    public class Creature : MonoBehaviour
    {
        public static float minSpeed = 1000;
        public static float maxSpeed = 0;

        public Transform body;
        public Transform lEye;
        public Transform rEye;
        public new SphereCollider collider;
        [Space(10)]
        public Color slowest = Color.blue;
        public Color fastest = Color.yellow;
        [Space(20)]
        public Species species;
        public Params parameters;
        [Space(10)]
        public int food = 0;
        public float energy = 30;
        public float baseEnergy = 30;
        public bool done = true;
        [Space(10)]
        public float deathTime = 0;
        public float distance = 45;
        [Space(20)]
        public bool activate = false;

        void Update()
        {
            if (!Application.isPlaying)
                ApplyParams();

            if (activate)
            {
                activate = false;
                Activate();
            }
        }

        void FixedUpdate()
        {
            if (deathTime <= Time.time || food == 2)
            {
                done = true;
            }

            if (!done)
            {
                transform.Translate(Vector3.forward * parameters.speed * 10 * Time.fixedDeltaTime);
                transform.Rotate(Vector3.up, Random.Range(-15, 15));

                if (distance < transform.position.magnitude)
                    transform.LookAt(Vector3.zero);
            }
        }

        public void Activate()
        {
            food = 0;
            done = false;

            CalculateLifetime();

            ApplyParams();

            transform.position = transform.position.normalized * distance;
            transform.LookAt(Vector3.zero);
        }

        public void CalculateLifetime()
        {
            energy = baseEnergy * (1 + Mathf.Sqrt(parameters.size));

            float lifetime = energy / GetEnegryUse();

            deathTime = Time.time + lifetime;
        }

        public float GetEnegryUse()
        {
            return Mathf.Pow(parameters.size, 3) * Mathf.Pow(parameters.speed, 2) + parameters.sense;
        }

        private readonly Vector3 bPos = new Vector3(0, 1.5f, 0);
        private readonly Vector3 bSize = new Vector3(2, 3, 2);
        private readonly Vector3 eSize = new Vector3(.2f, .2f, .2f);
        private readonly Vector3 lePos = new Vector3(-.6f, 2.4f, 1);
        private readonly Vector3 rePos = new Vector3(0.6f, 2.4f, 1);

        public void ApplyParams()
        {
            body.localPosition = parameters.size * bPos;
            body.localScale = parameters.size * bSize;

            lEye.localPosition = parameters.size * lePos;
            rEye.localPosition = parameters.size * rePos;
            lEye.localScale = parameters.sense * eSize;
            rEye.localScale = parameters.sense * eSize;

            if (Application.isPlaying)
            {
                float speed = parameters.speed;
                minSpeed = Mathf.Min(minSpeed, speed);
                maxSpeed = Mathf.Max(maxSpeed, speed);
                speed = (speed - minSpeed) / (maxSpeed - minSpeed);
                if (float.IsNaN(speed)) speed = 0;
                body.GetComponent<Renderer>().material.color = Color.Lerp(slowest, fastest, speed);
            }

            collider.radius = 2 + parameters.sense;
        }

        public void Mutate(float rate = 0.1f)
        {
            parameters.Mutate(rate);

            ApplyParams();
        }

        public void OnTriggerEnter(Collider other)
        {
            if (food == 2) return;

            if (other.gameObject.activeSelf)
            {
                if (other.tag == "food")
                {
                    food++;
                    other.gameObject.SetActive(false);
                    GameObject.Destroy(other.gameObject);
                    return;
                }

                Creature creature = other.GetComponent<Creature>();
                if (creature != null &&
                    creature.parameters.size + 0.5f < parameters.size)
                {
                    food++;

                    creature.food = 0;
                    creature.done = true;
                    other.gameObject.SetActive(false);
                }
            }
        }
    }
}

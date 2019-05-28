using System;
using System.Collections.Generic;
using UnityEngine;

namespace SimulationBase
{
    public class Mob : MonoBehaviour
    {
        public Transform body;
        public Transform lEye;
        public Transform rEye;
        public SphereCollider trigger;

        public event Action<Collider> TriggerEnter;
        public event Action<Collider> TriggerStay;
        public event Action<Collider> TriggerExit;

        public object controller;
        public MobBehaviour behaviour;

        private readonly Vector3 bPos = new Vector3(0, 1.5f, 0);
        private readonly Vector3 bSize = new Vector3(2, 3, 2);
        private readonly Vector3 eSize = new Vector3(.2f, .2f, .2f);
        private readonly Vector3 lePos = new Vector3(-.6f, 2.4f, 1);
        private readonly Vector3 rePos = new Vector3(0.6f, 2.4f, 1);

        public bool Active
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }

        public void ApplyParams(float size, float sense)
        {
            body.localPosition = size * bPos;
            body.localScale = size * bSize;

            lEye.localPosition = size * lePos;
            rEye.localPosition = size * rePos;
            lEye.localScale = sense * eSize;
            rEye.localScale = sense * eSize;

            trigger.radius = 2 + sense;
        }

        public void SetColor(Color color) { body.GetComponent<Renderer>().material.color = color; }

        public void OnTriggerEnter(Collider other) { TriggerEnter.SafeInvoke(other); }
        public void OnTriggerStay(Collider other) { TriggerStay.SafeInvoke(other); }
        public void OnTriggerExit(Collider other) { TriggerExit.SafeInvoke(other); }
    }

}
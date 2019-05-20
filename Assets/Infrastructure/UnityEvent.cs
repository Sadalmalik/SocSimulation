using System;
using System.Collections.Generic;
using UnityEngine;

public class UnityEvent
{
    internal class OnNextUpdateHandler
    {
        private Action action_;

        public OnNextUpdateHandler(Action action)
        {
            action_ = action;
            UnityEvent.Update += Update;
        }

        public void Update()
        {
            UnityEvent.Update -= Update;
            action_.SafeInvoke();
        }
    }

    public static event Action Update;
    public static event Action FixedUpdate;
    
    public static void Init()
    {
        var provider = GameObject.FindObjectOfType<UnityEventProvider>();

        if (provider==null)
            new GameObject("Unity Event Provider").AddComponent<UnityEventProvider>();
    }

    public static void OnNextUpdate(Action action)
    {
        new OnNextUpdateHandler(action);
    }

    public static void InvokeUpdate()
    {
        Update.SafeInvoke();
    }

    public static void InvokeFixedUpdate()
    {
        FixedUpdate.SafeInvoke();
    }
}

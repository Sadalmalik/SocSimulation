using System;
using UnityEngine;

public class UnityEvent
{
    public static event Action Update;
    public static event Action FixedUpdate;

    public static void Init()
    {
        var provider = GameObject.FindObjectOfType<UnityEventProvider>();

        if (provider==null)
            new GameObject("Unity Event Provider").AddComponent<UnityEventProvider>();
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

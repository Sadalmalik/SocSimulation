using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Delay
{
    private float endTime_;
    public float time;

    public Delay(float time=1)
    {
        this.time = time;
    }

    public void Start()
    {
        endTime_ = Time.time + time;
    }

    public bool IsComplete()
    {
        return endTime_ < Time.time;
    }
}

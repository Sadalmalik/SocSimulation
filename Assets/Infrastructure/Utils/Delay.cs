using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Delay
{
    private float endTime_;
    private float delay_;

    public Delay(float delay = 1)
    {
        endTime_ = float.MaxValue;
        delay_ = delay;
    }

    public void Set(float delay, bool restart=true)
    {
        delay_ = delay;
        if (restart) Start();
    }

    public void Start(float extra=0)
    {
        endTime_ = Time.time + delay_ + extra;
    }

    public bool IsComplete()
    {
        return endTime_ < Time.time;
    }

    public void AddTime(float time)
    {
        endTime_ += time;
    }

    public void Stop()
    {
        this.endTime_ = float.MaxValue;
    }
}

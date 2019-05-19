using System;
using System.Collections.Generic;
using UnityEngine;

public class Transition
{
    public float sleep;
    public string target;
    public Func<bool> predicate;

    public Action before;
    public Action between;
    public Action after;
}

public class State
{
    public string name;
    public Action onEnter;
    public Action onUpdate;
    public Action onExit;

    public List<Transition> transitions;

    public State(string name)
    {
        this.name = name;
        transitions = new List<Transition>();
    }

    public State AddTransition(string target, Func<bool> predicate, Action before = null, Action between = null, Action after = null, float sleep = 0)
    {
        transitions.Add(new Transition()
        {
            sleep = sleep,
            target = target,
            predicate = predicate,

            before = before,
            between = between,
            after = after
        });
        return this;
    }

    public State OnEnter(Action onEnterHandler) { onEnter = onEnterHandler; return this; }
    public State OnUpdate(Action onUpdateHandler) { onUpdate = onUpdateHandler; return this; }
    public State OnExit(Action onExitHandler) { onExit = onExitHandler; return this; }

    public void InvokeEnter() { onEnter.SafeInvoke(); }
    public void InvokeUpdate() { onUpdate.SafeInvoke(); }
    public void InvokeExit() { onExit.SafeInvoke(); }

}

public class StateMachine
{
    private bool debug_;
    private bool isWorking_;
    private State currentState_;
    private Dictionary<string, State> states_;

    private Transition delayedTransition_;
    private float nextTime_;

    public string Name { get; private set; }
    public string State { get { return currentState_ == null ? "null" : currentState_.name; } }

    public StateMachine(string name, bool debug = false)
    {
        Name = name;
        debug_ = debug;
        isWorking_ = false;
        currentState_ = null;
        states_ = new Dictionary<string, State>();
        delayedTransition_ = null;
        nextTime_ = 0;
    }

    public State AddState(string name, Action onEnterHandler = null, Action onUpdateHandler = null, Action onExitHandler = null)
    {
        State s = new State(name);
        states_.Add(name, s);
        if (null != onEnterHandler) s.OnEnter(onEnterHandler);
        if (null != onUpdateHandler) s.OnUpdate(onUpdateHandler);
        if (null != onExitHandler) s.OnExit(onExitHandler);
        return s;
    }

    public void Start(string name)
    {
        ChangeState(name);

        if (!isWorking_)
        {
            isWorking_ = true;
            UnityEvent.Update += Update;
        }
    }

    public void Stop()
    {
        ChangeState(null);

        if (isWorking_)
        {
            isWorking_ = false;
            UnityEvent.Update -= Update;
        }
    }

    private void ChangeState(string name, Transition transition = null, bool skipDelay=false)
    {
        if (debug_)
            Log.Info("[StateMachine {0}]: Change state from '{1}' to '{2}'",
                Name, currentState_ != null ? currentState_.name : "", name);

        if (!skipDelay)
        {
            if (transition != null)
                transition.before.SafeInvoke();

            if (null != currentState_)
            {
                currentState_.InvokeExit();
                currentState_ = null;
            }

            if (transition != null)
                if (transition.sleep > 0)
                {
                    nextTime_ = Time.time + transition.sleep;
                    delayedTransition_ = transition;
                    return;
                }
        }
        
        if (transition != null)
            transition.between.SafeInvoke();

        if (name != null)
        {
            if (!states_.TryGetValue(name, out currentState_))
                throw new Exception(string.Format("[StateMachine {0}]: dose not contains state '{0}'", Name, name));
            currentState_.InvokeEnter();
        }

        if (transition != null)
            transition.after.SafeInvoke();
    }

    private void Update()
    {
        if (delayedTransition_!=null)
        {
            if (nextTime_<Time.time)
            {
                //  Complete transition
                ChangeState(delayedTransition_.target, delayedTransition_, true);
                delayedTransition_ = null;
            }

            return;
        }

        if (null != currentState_)
        {
            currentState_.InvokeUpdate();

            foreach (var trans in currentState_.transitions)
                if (trans.predicate())
                {
                    ChangeState(trans.target, trans);
                    break;
                }
        }
        else
        {
            throw new Exception(string.Format("[StateMachine {0}]: reach null state!", Name));
        }
    }
}

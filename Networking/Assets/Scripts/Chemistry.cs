using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chemistry : MonoBehaviour {
    //State of Object
    public enum State { Default, Frozen, OnFire, Shocked, Destroy };
    private State m_MyState;

    public State GetState() { return m_MyState; }
    public void SetState(State _state) { m_MyState = _state; }

    public abstract void EvaluateChange();
}

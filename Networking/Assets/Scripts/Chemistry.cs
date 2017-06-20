using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Chemistry : MonoBehaviour {
    //State of Object
    public enum State { None, Frozen, Burning, Shocked };
    private State m_MyState;

    public State GetState() { return m_MyState; }
    public void ChangeState(State _state) { m_MyState = _state; }

    public abstract void ChemicalReaction(State _state);
}

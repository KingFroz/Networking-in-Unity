using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehavior : Chemistry {
    public enum MaterialType { Earth, Wood, Metal };

    [SerializeField]
    private MaterialType m_Type;

    public override void ChemicalReaction(State _state)
    {
        //Early exit Case
        if (_state == State.None || GetState() == _state) {
            return;
        }

        //Based on Material Type, evaluate reacion
        switch (m_Type)
        {
            case MaterialType.Earth:
                break;
            case MaterialType.Wood:
                if (_state ==  State.Burning) {
                    Debug.Log("Wood Object set on Fire");
                    break;
                }
                if (_state == State.Frozen) {
                    Debug.Log("Wood Object was Frozen");
                    break;
                }
                break;
            case MaterialType.Metal:
                if (_state == State.Shocked)
                {
                    Debug.Log("Electrified");
                }
                break;
            default:
                break;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Chemistry temp = other.gameObject.GetComponent<Chemistry>();
        if (temp != null)
            ChemicalReaction(temp.GetState());
    }
}

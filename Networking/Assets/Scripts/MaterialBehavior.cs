using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehavior : Chemistry {
    public enum MaterialType { Earth, Wood, Metal, Water };

    [SerializeField]
    private MaterialType m_Type;

    public override void ChemicalReaction(State _state)
    {
        switch (m_Type)
        {
            case MaterialType.Earth:
                break;
            case MaterialType.Wood:
                if (_state ==  State.Burning)
                {
                    Debug.Log("Wood Object set on Fire");
                }
                
                break;
            case MaterialType.Metal:
                break;
            case MaterialType.Water:
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

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBehavior : Chemistry {

    public enum Element { Fire, Water, Air, Earth, Electricity, Wind };

    [SerializeField]
    private Element m_Element = Element.Fire;

    void InitializeElement()
    {
        switch (m_Element)
        {
            case Element.Fire:
                ChangeState(State.Burning);
                break;
            case Element.Water:
                break;
            case Element.Air:
                break;
            case Element.Earth:
                break;
            case Element.Electricity:
                break;
            case Element.Wind:
                break;
            default:
                break;
        }
    }

    void Start()
    {
        InitializeElement();
    }

    public override void ChemicalReaction(State _state)
    {
        switch (m_Element)
        {
            case Element.Fire:
                break;
            case Element.Water:
                break;
            case Element.Air:
                break;
            case Element.Earth:
                break;
            case Element.Electricity:
                break;
            case Element.Wind:
                break;
            default:
                break;
        }
    }
}

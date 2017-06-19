using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementBehavior : Chemistry {

    public enum Element { Fire, Water, Air, Earth, Electricity, Wind };

    [SerializeField]
    private Element m_Element;

    public override void EvaluateChange()
    {
        throw new NotImplementedException();
    }
}

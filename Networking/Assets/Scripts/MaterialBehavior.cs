using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialBehavior : Chemistry {
    public enum MaterialType { Earth, Wood, Metal, Water };

    [SerializeField]
    private MaterialType m_Type;

    public override void EvaluateChange()
    {
        throw new NotImplementedException();
    }
}

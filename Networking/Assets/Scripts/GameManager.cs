using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [System.Serializable]
    public class Reserve
    {
        public GameObject gObject;
        public int amount;
        Reserve(GameObject _object, int _amount)
        {
            gObject = _object;
            amount = _amount;
        }
    }

    public List<Reserve> objectsToReserve;

    // Use this for initialization
    void Start () {
        foreach (Reserve item in objectsToReserve)
        {
            PoolManager.instance.CreatePool(item.gObject, item.amount);
        }
    }
}

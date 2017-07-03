using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    //(int)Instance ID of Prefab, (Queue)Objects in that pool
    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();

    //Make PoolManager a Singleton
    static PoolManager _instance;
    public static PoolManager instance
    {
        get {
            if (_instance == null) {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    //Takes in a (poolSize)number of GameObjects(Prefab)
    public void CreatePool(GameObject prefab, int poolSize)
    {
        //GetInstanceID returns a unique integer for every GameObject
        int poolKey = prefab.GetInstanceID();

        GameObject poolHolder = new GameObject(prefab.name + " pool");
        poolHolder.transform.parent = transform;

        //Make sure poolKey isn't already in Dictionary
        if (!poolDictionary.ContainsKey(poolKey)) {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());

            //Instantiate a set amount(poolSize) of Objects
            for (int i = 0; i < poolSize; i++)
            {
                ObjectInstance newObj = new ObjectInstance(Instantiate(prefab) as GameObject);
                poolDictionary[poolKey].Enqueue(newObj);
                newObj.SetParent(poolHolder.transform);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 pos, Quaternion rot, Vector3 scale)
    {
        int poolKey = prefab.GetInstanceID();

        //Exist in our Dictionary?
        if (poolDictionary.ContainsKey(poolKey)) {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();
            poolDictionary[poolKey].Enqueue(objectToReuse);

            objectToReuse.Reuse(pos, rot, scale);
        }
    }

    public class ObjectInstance
    {
        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScript;

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScript = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 pos, Quaternion rot, Vector3 scale)
        {
            gameObject.SetActive(true);
            transform.position = pos;
            transform.rotation = rot;
            transform.localScale = scale;

            if (hasPoolObjectComponent)
                poolObjectScript.OnObjectReuse();
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}

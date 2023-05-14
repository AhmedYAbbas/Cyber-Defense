using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{ 
    private PoolableObject _prefab;
    private List<PoolableObject> _availableObjects;

    private ObjectPool(PoolableObject prefab, int size)
    {
        this._prefab = prefab;
        _availableObjects = new List<PoolableObject>(size);
    }

    public static ObjectPool CreatInstance(PoolableObject prefab, int size)
    {
        ObjectPool pool = new ObjectPool(prefab, size);
        GameObject poolObject = new GameObject(prefab.name + "Pool");
        pool.CreateObjects(poolObject.transform, size);
        return pool;
    }

    void CreateObjects(Transform parent,int size)
    {
        for (int i = 0; i < size; i++)
        {
            PoolableObject poolableObject =
                GameObject.Instantiate(_prefab, Vector3.zero, Quaternion.identity, parent.transform);
            poolableObject.Parent = this;
            poolableObject.gameObject.SetActive(false);
        }
    }

    public void ReturnObjectToPool(PoolableObject poolableObject)
    {
        _availableObjects.Add(poolableObject);
    }

    public PoolableObject GetObject()
    {
        if (_availableObjects.Count > 0)
        {
            PoolableObject instance = _availableObjects[0];
            _availableObjects.RemoveAt(0);
            instance.gameObject.SetActive(true);
            return instance;
        }
        else
        {
            return null;
        }
    }
}

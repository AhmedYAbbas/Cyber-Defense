using Photon.Pun;
using System;
using UnityEngine;


public class PoolableObject : MonoBehaviourPunCallbacks
{
    public ObjectPool Parent;

    public virtual void OnDisable()
    {
        Parent.ReturnObjectToPool(this);
    }
}
using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class MoveToTarget : MonoBehaviour
{
    // Private
    private NavMeshAgent _agent;
    private GameObject _target;
    private Vector3 _closestTower;
    private List<GameObject> _towers;
    private GameObject _currentTarget;
    private float _attackCountdown;
    private float _attackSpeed = 1;
    private int _damage = 10;
    private ObjectPool _projectilePool;

    void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Base");
    }
    private void OnEnable()
    {
        if (_target != null)
        {
            print("Target Found");
            _agent.SetDestination(_target.transform.position);
        }
    }
 
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    private NavMeshAgent _agent;
    private GameObject _target;
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Tower");
        if (_target != null)
        {
            print("Target Found");
            _agent.SetDestination(_target.transform.position);
        }
    }
}

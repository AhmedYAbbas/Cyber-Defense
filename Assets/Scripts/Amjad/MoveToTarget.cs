using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTarget : MonoBehaviour
{
    // Private
    private NavMeshAgent _agent;
    private GameObject _target;
    //private Vector3 _closestTower;
    private List<Vector3> _towers;

    void Awake()
    {
        _towers = new List<Vector3>();
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

    void OnTriggerEnter(Collider collision)
    {
        _towers.Add(collision.transform.position);
        print("Tower foundddddddddddddddddd");
    }

    void OnTriggerStay(Collider collision)
    {
        if (collision.transform.CompareTag("Tower"))
        {
            foreach (Vector3 tower in _towers)
            {
                print("Attackinngggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
            }
        }
    }

    void OnTriggerExit(Collider collision)
    {
        _towers.Remove(collision.transform.position);
    }
}
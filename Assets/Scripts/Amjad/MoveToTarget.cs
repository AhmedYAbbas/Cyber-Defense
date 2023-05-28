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


    [SerializeField] private PoolableObject _malwareProjectilePrefab;


    void Awake()
    {
        //_towers = new List<GameObject>();
        _agent = GetComponent<NavMeshAgent>();
        _target = GameObject.FindGameObjectWithTag("Base");
        print(_target.name + " found");
        //_projectilePool = ObjectPool.CreatInstance(_malwareProjectilePrefab, 20);
    }

    private void Update()
    {
        //GettingTheMostDangerousTower();
        //GettingTheClosestTower();
        //CalculateShootingRate();
        //print(_towers.Count);
    }
    private void CalculateShootingRate()
    {
        if (_attackCountdown <= 0)
        {
            Shoot();
            _attackCountdown = 1f / _attackSpeed;
        }

        _attackCountdown -= Time.deltaTime;
    }

    private void Shoot()
    {
        print("trying to shoot");
        if (_towers.Count > 0)
        {
            //GameObject projectile = Instantiate(_projectilePrefab, transform.position , Quaternion.identity);
            var projectile = _projectilePool.GetObject();
            projectile.transform.position = transform.position;
            projectile.GetComponent<MalwareProjectile>().GetTarget(_currentTarget.transform, _damage);
        }
    }
    private void GettingTheClosestTower()
    {
        float closestTower = 0;
        GameObject tempEnemy = null;
        for (int i = 0; i < _towers.Count; i++)
        {
            print("loop in " + i);
            if (_towers[i] == null)
            {
                _towers.RemoveAt(i);
                continue;
            }
            //float enemyDistanceFromTower = Vector3.Distance(transform.position, _enemies[i].transform.position);
            var test = _towers[i].GetComponent<Malware>();
            //float currentDangerousLevel = (-i+1) * 0.6f + (0.2f * test.MovementSpeed);
            float currentClosestTower = Vector3.Distance(transform.position, _towers[i].transform.position);
            if (closestTower < currentClosestTower)
            {
                closestTower = currentClosestTower;
                tempEnemy = _towers[i];
            }

        }
        _currentTarget = tempEnemy;
    }

    private void OnEnable()
    {
        if (_target != null)
        {
            print("Target Found");
            _agent.SetDestination(_target.transform.position);
        }
    }

    /*void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Tower"))
        {
            _towers.Add(collision.gameObject);
            print("Tower foundddddddddddddddddd");
        }
    }*/

    /*void OnTriggerStay(Collider collision)
    {
        if (collision.transform.CompareTag("Tower"))
        {
            foreach (Vector3 tower in _towers)
            {
                print("Attackinngggggggggggggggggggggggggggggggggggggggggggggggggggggggggggggg");
            }
        }
    }*/

    /*void OnTriggerExit(Collider collision)
    {
        if (collision.CompareTag("Tower"))
        {
            _towers.Remove(collision.gameObject);
            print("Tower Gryyyyyyyyyyyy");

        }
    }*/
}
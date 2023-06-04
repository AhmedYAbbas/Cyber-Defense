using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class TowerShootingSystem : MonoBehaviourPunCallbacks
{
    [SerializeField] private Tower _towerScript;
    private TowerModifications _towerModifications;
    
    private float _range;
    private int _damage;
    private float _attackSpeed;
    private float _attackCountdown;
    private SphereCollider _targetingCollider;
    private PoolableObject _towerProjectilePrefab;
    private ObjectPool _projectilePool;
    private GameObject _currentTarget;
    private List<Malware> _enemies = new List<Malware>();
    
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private Transform towerHead;
    [SerializeField] private float towerHeadRotationSpeed;

    [SerializeField] private GameObject muzzleGameObject;
    private ParticleSystem _muzzleParticleSystem;
    private void Awake()
    {
        _towerScript.TowerGotModified += GetTowerModification;
        _targetingCollider = GetComponent<SphereCollider>();
        _muzzleParticleSystem = muzzleGameObject.GetComponent<ParticleSystem>();
        _muzzleParticleSystem.Stop();
    }

    private void Update()
    {
        if (_enemies.Count > 0)
        {
            GettingTheMostDangerousEnemy();
            RotateTheTowerHead();
            CalculateShootingRate();
        }
    }

    void GetTowerModification(object sender , EventArgs e)
    {
       _towerModifications = _towerScript.currentModifications;
       UpdateTowerStats();
    }

    private void UpdateTowerStats()
    {
        _range = _towerModifications.range;
        _damage = _towerModifications.damage;
        _attackSpeed = _towerModifications.attackSpeed;
        _towerProjectilePrefab = _towerModifications.bulletPrefab;
        _targetingCollider.radius = _range;
        _projectilePool = ObjectPool.CreatInstance(_towerProjectilePrefab, 20);
    }
    private void GettingTheMostDangerousEnemy()
    {
        float dangerousLevel = float.MinValue;
        GameObject tempEnemy = null;
        for (int i = 0 ; i < _enemies.Count ;i++)
        {
            if (!_enemies[i].gameObject.activeInHierarchy)
            {
                _enemies.RemoveAt(i);
                continue;
            }
            float currentDangerousLevel = (-i + 1) * 0.6f + (0.5f * _enemies[i].MovementSpeed);
            if (dangerousLevel < currentDangerousLevel)
            {
                dangerousLevel = currentDangerousLevel;
                tempEnemy = _enemies[i].gameObject;
            }
        }
        _currentTarget = tempEnemy;
    }
    private void RotateTheTowerHead()
    {
        if (_currentTarget != null)
        {
            Vector3 dir = _currentTarget.transform.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            Vector3 rotation = Quaternion.Lerp(towerHead.rotation,lookRotation,Time.deltaTime * towerHeadRotationSpeed).eulerAngles;
            towerHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }
    private void CalculateShootingRate()
    {
        if (_enemies.Count > 0)
        {
            if (_attackCountdown <= 0)
            {
                Shoot();
                _attackCountdown = 1f / _attackSpeed;
            }

            _attackCountdown -= Time.deltaTime;
        }
    }
    
    private void Shoot()
    {
          var projectile = _projectilePool.GetObject() as TowerHomingProjectile;
          _muzzleParticleSystem.Play();
          projectile.transform.position = shootingPoint.position;
          projectile.GetTarget(_currentTarget.transform,_damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") )
        {
            _enemies.Add(other.GetComponent<Malware>());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") )
        {
            _enemies.Remove(other.GetComponent<Malware>());
        }
    }

    
}

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
    private List<GameObject> _enemies = new List<GameObject>();
    
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
        GettingTheMostDangerousEnemy();
        RotateTheTowerHead();
        CalculateShootingRate();
        print(_enemies.Count);
        
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
            print("loop in "+ i );
            if (!_enemies[i].gameObject.activeInHierarchy)
            {
                _enemies.RemoveAt(i);
                continue;
            }
            //float enemyDistanceFromTower = Vector3.Distance(transform.position, _enemies[i].transform.position);
            //var test = _enemies[i].GetComponent<Malware>();
            float currentDangerousLevel = (-i + 1) * 0.6f + (0.2f);// * test.MovementSpeed) ;
            if (dangerousLevel < currentDangerousLevel)
            {
                dangerousLevel = currentDangerousLevel;
                tempEnemy = _enemies[i];
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
        if (_enemies.Count > 0 && _currentTarget != null)
        {
            if (_attackCountdown <= 0)
            {
                Shoot();
                //photonView.RPC(nameof(Shoot), RpcTarget.All);
                _attackCountdown = 1f / _attackSpeed;
            }

            _attackCountdown -= Time.deltaTime;
        }
    }
    
    //[PunRPC]
    private void Shoot()
    {
          var projectile = _projectilePool.GetObject();
          _muzzleParticleSystem.Play();
          projectile.transform.position = shootingPoint.position;
          projectile.GetComponent<TowerHomingProjectile>().GetTarget(_currentTarget.transform,_damage);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy")&& !_enemies.Contains(other.gameObject))
        {
            _enemies.Add(other.gameObject);
            print("enemy entered the range");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") && _enemies.Contains(other.gameObject))
        {
            _enemies.Remove(other.gameObject);
            print("enemy exited the range");
        }
    }

    
}

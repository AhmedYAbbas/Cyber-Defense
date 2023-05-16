using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Tower : MonoBehaviour
{
    
    private List<GameObject> _enemies = new List<GameObject>();
    private float _attackCountdown;
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    [SerializeField] private float _range;
    [SerializeField] private int _damage;
    [SerializeField] private float _attackSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Transform towerHead;
    [SerializeField] private Transform _towerIconPostion;
    private GameObject _currentTowerIcon;
    private GameObject _towerIcon;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private PoolableObject _TowerProjectilePrefab;
    private BoxCollider _targetingCollider;
    

    private ObjectPool _projectilePool;
    private GameObject _currentTarget;
    [SerializeField] private TowerModifications baseTowerSO;
    [SerializeField] private TowerModifications ScriptableObjectTest;
    
    void Awake()
    {
        _targetingCollider = GetComponent<BoxCollider>();
        ModifyTower(baseTowerSO);
        _currentHealth = _maxHealth;
        _projectilePool = ObjectPool.CreatInstance(_TowerProjectilePrefab, 100);
        //InvokeRepeating("GettingTheMostDangerousEnemy", 0, 0.2f);
    }
    
    void Update()
    {
        GettingTheMostDangerousEnemy();
        RotateTheTowerHead();
        CalculateShootingRate();
    }
    
    public void ModifyTower(TowerModifications towerModifications)
    {
        _maxHealth = towerModifications.maxHealth;
        _range = towerModifications.range;
        _damage = towerModifications.damage;
        _attackSpeed = towerModifications.attackSpeed;
        _TowerProjectilePrefab = towerModifications.bulletPrefab;
        _towerIcon = towerModifications.towerIcon;
        //_targetingCollider.radius = _range;
        SwitchTowerIcon();
    }
    [ContextMenu("ScriptableObjectTest")]
    public void ScriptableObjectTestfun()
    {
        _maxHealth = ScriptableObjectTest.maxHealth;
        _range = ScriptableObjectTest.range;
        _damage = ScriptableObjectTest.damage;
        _attackSpeed = ScriptableObjectTest.attackSpeed;
        _TowerProjectilePrefab = ScriptableObjectTest.bulletPrefab;
        _towerIcon = ScriptableObjectTest.towerIcon;
        //_targetingCollider.radius = _range;
        SwitchTowerIcon();
    }

    public void DamageTower(int dmg)
    {
        _currentHealth -= dmg;
        if (_currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void SwitchTowerIcon()
    {
        if (_currentTowerIcon != null)
        {
            DestroyImmediate(_currentTowerIcon);
        }
        _currentTowerIcon = Instantiate(_towerIcon, _towerIconPostion.transform.position, quaternion.identity,towerHead);
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
        if (_enemies.Count > 0)
        {
            //var projectile = Instantiate(projectilePrefab, shootingPoint.position , Quaternion.identity);
            var projectile = _projectilePool.GetObject();
            projectile.transform.position = shootingPoint.position;
            projectile.GetComponent<TowerHomingProjectile>().GetTarget(_currentTarget.transform,_damage);
        }
    }

    private void GettingTheMostDangerousEnemy()
    {
        float dangerousLevel = float.MinValue;
        GameObject tempEnemy = null;
        int enemyCount = _enemies.Count;
        for (int i = 0; i < enemyCount; i++)
        {
            print("loop in " + i);
            if (_enemies[i] == null || Vector3.Distance(transform.position, _enemies[i].transform.position) > 500)
            {
                //print("inside shitty shit");////yes meee samyyyy
                _enemies.RemoveAt(i);
                enemyCount--;
                continue;
            }
            //float enemyDistanceFromTower = Vector3.Distance(transform.position, _enemies[i].transform.position);
            var test = _enemies[i].GetComponent<Malware>();
            float currentDangerousLevel = (-i + 1) * 0.6f + (0.2f * test.MovementSpeed);
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
            Vector3 rotation = Quaternion.Lerp(towerHead.rotation,lookRotation,Time.deltaTime * _rotationSpeed).eulerAngles;
            towerHead.rotation = Quaternion.Euler(0f, rotation.y, 0f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print("found enemy");
        if (other.CompareTag("Enemy"))
        {
            _enemies.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("enemy Scaped or died idk");
        if (other.CompareTag("Enemy"))
        {
            _enemies.Remove(other.gameObject);
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,_range);
        if (_currentTarget != null)
        {
            Gizmos.DrawLine(transform.position,_currentTarget.transform.position);
        }
    }
}

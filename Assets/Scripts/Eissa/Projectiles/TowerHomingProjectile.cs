using System;
using UnityEngine;

public class TowerHomingProjectile : PoolableObject
{
    private Transform _target;
    private Vector3 _targetDir;
    [SerializeField] private AnimationCurve positionCurve;
    private Coroutine _homingCoroutine;
    private int _damage;
    [SerializeField] float projectileSpeed = .1f;

    public override void OnDisable()
    {
        base.OnDisable();
        _targetDir = Vector3.zero;
        _target = null;
    }

    public void GetTarget(Transform target,int dmg)
    {
        _target = target;
        _damage = dmg;
    }

    private void Update()
    {
       
        if (_target.gameObject.activeInHierarchy)
        {
            _targetDir = (_target.position - transform.position).normalized;
            transform.position += _targetDir * (projectileSpeed * Time.deltaTime);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && _target.gameObject.activeInHierarchy)
        {
            if (other != null && other.gameObject.activeInHierarchy)
            {
                other.GetComponent<Malware>().DamageMalware(_damage);
                this.gameObject.SetActive(false);
            }
            else
            {
                this.gameObject.SetActive(false);
            }
        }
        else
        {
           // gameObject.SetActive(false);
        }
    }
}

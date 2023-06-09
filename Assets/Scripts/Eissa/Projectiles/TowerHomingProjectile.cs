using System;
using Photon.Pun;
using UnityEngine;

public class TowerHomingProjectile : PoolableObject
{

    private Transform _target;
    private Vector3 _targetDir;
    private int _damage;
    [SerializeField] private float projectileSpeed;

    public override void OnDisable()
    {
        base.OnDisable();
        _targetDir = Vector3.zero;
        _target = null;
    }

    public void GetTarget(Transform target, int dmg)
    {
        _target = target;
        _damage = dmg;
    }

    private void Update()
    {
        if (_target != null && _target.gameObject.activeInHierarchy)
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
        if (other.CompareTag("Enemy") && other.gameObject != null && other.gameObject.activeInHierarchy)
        {
            var malwareScript = other.GetComponent<Malware>();
            if (malwareScript.photonView.IsMine)
            {
                malwareScript.DamageMalware(_damage);
            }

            if (other.TryGetComponent(out DamageTextAnimation damageText))
            {
                damageText.GotDamaged(_damage);
            }

            this.gameObject.SetActive(false);
        }
    }
}

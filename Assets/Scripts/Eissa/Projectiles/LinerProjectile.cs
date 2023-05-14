using UnityEngine;

 public class LinerProjectile : PoolableObject
 {
     private Transform _target;
     private Vector3 _targetDir;
     private int _damage;
     [SerializeField] float projectileSpeed = 1f;

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
         if (_target != null)
         {
             transform.position += _targetDir * (projectileSpeed * Time.deltaTime);
         }
     }

     private void OnTriggerEnter(Collider other)
     {
         if (other.CompareTag("Enemy"))
         {
             Destroy(other.gameObject);
             gameObject.SetActive(false);
         }
         else
         {
             gameObject.SetActive(false);
         }
     }
 }

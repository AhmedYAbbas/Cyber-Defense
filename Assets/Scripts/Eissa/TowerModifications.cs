using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "TowerModifications", menuName = "Towers", order = 0)]
public class TowerModifications : ScriptableObject
{
        public string description;
        public float maxHealth;
        public int damage;
        public int range;
        public float attackSpeed;
        public PoolableObject bulletPrefab;
        public GameObject towerIcon;
}

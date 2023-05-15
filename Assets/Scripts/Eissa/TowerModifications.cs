using UnityEngine;
using UnityEngine.Serialization;


[CreateAssetMenu(fileName = "TowerModifications", menuName = "Towers", order = 0)]
public class TowerModifications : ScriptableObject
{
        public string description;
        public string ModificationName;
        public float maxHealth;
        public int damage;
        public int range;
        public int EnergyCost;
        public float attackSpeed;
        public PoolableObject bulletPrefab;
        public GameObject towerIcon;
}

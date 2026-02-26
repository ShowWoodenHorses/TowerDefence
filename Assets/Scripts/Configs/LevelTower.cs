using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "LevelTower", menuName = "Scriptable Objects/Level Tower")]
    public class LevelTower : ScriptableObject
    {
        public GameObject obj;
        public float attackRadius;
        public float attackCooldown;
        public int damage;
        public float speedProjectile;
        public EnemyType targetTypes;

    }
}
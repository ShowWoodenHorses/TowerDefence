using System.Collections;
using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Visuals
{
    public enum TowerType
    {
        Basic,
    }
    public class TowerVisual : MonoBehaviour
    {
        [SerializeField] private TowerType towerType;
        [SerializeField] private ProjectileType projectileType;
        [SerializeField] private EnemyType targetTypes;

        public TowerType GetTowerType() => towerType;

        public EnemyType GetTargetTypes() => targetTypes;

        public ProjectileType GetProjectileType() => projectileType;
    }
}
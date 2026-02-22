using System.Collections;
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

        public TowerType GetTowerType() => towerType;

        public ProjectileType GetProjectileType() => projectileType;
    }
}
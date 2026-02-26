using System.Collections;
using Assets.Scripts.Configs;
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
        [SerializeField] private GradationTower gradationTower;

        private GameObject currentVisualObject = null;

        public TowerType GetTowerType() => towerType;

        public EnemyType GetTargetTypes() => targetTypes;

        public ProjectileType GetProjectileType() => projectileType;

        public GradationTower GetGradationTower() => gradationTower;

        public void SetVisualObject(GameObject newVisual)
        {
            if (currentVisualObject != null)
            {
                var spawnPos = currentVisualObject.transform.position;
                Destroy(currentVisualObject);

                currentVisualObject = Instantiate(newVisual, spawnPos, Quaternion.identity);
                return;
            }

            currentVisualObject = Instantiate(newVisual, transform.position, Quaternion.identity);
        }
    }
}
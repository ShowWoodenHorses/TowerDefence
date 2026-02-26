using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Building;
using Assets.Scripts.Visuals;
using Assets.Scripts.Data;
using Assets.Scripts.Configs;

namespace Assets.Scripts.Managers
{
    public class BuildManager : MonoBehaviour
    {
        [SerializeField] private List<TowerItem> towerItems;

        private TowerManager towerManager;

        public void Init(TowerManager towerManager)
        {
            towerItems.ForEach(t => t.Init(OnCreateTower));
            this.towerManager = towerManager;
        }

        private void OnCreateTower(GameObject tower, Vector2 position)
        {
            Ray ray = Camera.main.ScreenPointToRay(position);
            RaycastHit hit;
            
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 worldPos = hit.point;
                Collider[] colliders = Physics.OverlapSphere(worldPos, 0.5f);

                foreach (Collider collider in colliders)
                {
                    TowerCell towerCell = collider.transform.GetComponent<TowerCell>();

                    if (towerCell != null && !towerCell.HasTower())
                    {
                        towerCell.BuildTower(towerManager.GetCurrentTowerIndex());
                        var createdTower = Instantiate(tower, towerCell.GetPosition(), Quaternion.identity);
                        TowerVisual towerVisual = createdTower.GetComponent<TowerVisual>();
                        LevelTower config = towerVisual.GetGradationTower().levels[0];
                        towerVisual.SetVisualObject(config.obj);

                        TowerData data = new TowerData
                        {
                            position = towerCell.GetPosition(),
                            attackRadius = config.attackRadius,
                            attackCooldown = config.attackCooldown,
                            timeSinceLastAttack = 0f,
                            damage = config.damage,
                            speed = config.speedProjectile,
                            targetMask = (int)towerVisual.GetTargetTypes(),
                            level = 0,
                        };

                        towerManager.AddTower(data, towerVisual);

                        break;
                    }
                }
            }            
        }
    }
}
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

        private void OnCreateTower(GradationTower gradationTower, Vector2 position)
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
                        LevelTower config = gradationTower.levels[0];

                        TowerData data = new TowerData
                        {
                            position = towerCell.GetPosition(),
                            timeSinceLastAttack = 0f,
                            level = 0,
                        };
                        data.ApplyLevel(config);

                        towerManager.AddTower(data, config, gradationTower);

                        break;
                    }
                }
            }            
        }
    }
}
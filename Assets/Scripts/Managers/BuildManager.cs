using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Building;
using Assets.Scripts.Visuals;
using Assets.Scripts.Data;

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

                        TowerData data = new TowerData
                        {
                            position = towerCell.GetPosition(),
                            attackRadius = 25f,
                            attackCooldown = 1f,
                            timeSinceLastAttack = 0f,
                            damage = 200,
                            speed = 50,
                            targetMask = (int)towerVisual.GetTargetTypes()
                        };

                        towerManager.AddTower(data, towerVisual);

                        break;
                    }
                }
            }            
        }
    }
}
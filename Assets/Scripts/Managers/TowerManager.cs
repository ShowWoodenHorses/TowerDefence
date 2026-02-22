using Assets.Scripts.Data;
using Assets.Scripts.Visuals;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Managers
{
    public class TowerManager : MonoBehaviour
    {
        public TowerData[] towers;
        public TowerVisual[] towersVisual;

        private int countTower;

        private EnemyManager enemyManager;
        private GridManager gridManager;
        private ProjectileManager projectileManager;

        private int[] enemySearchBuffer;

        public void Init(
            EnemyManager enemyManager,
            GridManager gridManager,
            ProjectileManager projectileManager)
        {
            this.enemyManager = enemyManager;
            this.gridManager = gridManager;
            this.projectileManager = projectileManager;

            towers = new TowerData[64];
            towersVisual = new TowerVisual[64];
            enemySearchBuffer = new int[256];
        }

        void Update()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < towers.Length; i++)
            {
                ref TowerData tower = ref towers[i];

                tower.timeSinceLastAttack += dt;

                if (tower.timeSinceLastAttack < tower.attackCooldown)
                    continue;

                int closestIndex = FindClosestEnemy(tower.position, tower.attackRadius);

                if (closestIndex != -1)
                {
                    ShootAt(closestIndex, tower, towersVisual[i]);
                    tower.timeSinceLastAttack = 0f;
                }
            }
        }

        public int GetCurrentTowerIndex() => countTower;

        public void AddTower(TowerData data, TowerVisual visual)
        {
            towers[countTower] = data;
            towersVisual[countTower] = visual;

            countTower++;
        }

        public void RemoveTower(int towerIndex)
        {
            towers[towerIndex] = towers[countTower - 1];
            towersVisual[towerIndex] = towersVisual[countTower - 1];

            countTower--;
        }

        int FindClosestEnemy(Vector3 position, float radius)
        {
            int foundCount = gridManager.GetEnemiesInRadiusNonAlloc(
                position,
                radius,
                enemySearchBuffer);

            int closestIndex = -1;
            float minDist = radius * radius;

            for (int i = 0; i < foundCount; i++)
            {
                int idx = enemySearchBuffer[i];

                if (idx >= enemyManager.countEnemies)
                    continue;

                ref EnemyData e = ref enemyManager.enemies[idx];

                float dist = (e.position - position).sqrMagnitude;

                if (dist < minDist)
                {
                    minDist = dist;
                    closestIndex = idx;
                }
            }

            return closestIndex;
        }

        void ShootAt(int enemyIndex, TowerData data, TowerVisual visual)
        {
            projectileManager.Spawn(
                visual.GetProjectileType(),
                data.position,
                enemyIndex,
                data.speed,
                data.damage
            );
        }
    }
}
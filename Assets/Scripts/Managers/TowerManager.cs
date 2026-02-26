using Assets.Scripts.Data;
using Assets.Scripts.Visuals;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Configs;

namespace Assets.Scripts.Managers
{
    public class TowerManager : MonoBehaviour
    {
        public TowerData[] towers;
        public TowerVisual[] towersVisual;
        public GradationTower[] towersGradation;

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
            towersGradation = new GradationTower[64];
            enemySearchBuffer = new int[256];
        }

        public TowerData GetTowerData(int index) => towers[index];

        public GradationTower GetGradationTower(int index) => towersGradation[index];

        void Update()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < towers.Length; i++)
            {
                ref TowerData tower = ref towers[i];

                tower.timeSinceLastAttack += dt;

                if (tower.timeSinceLastAttack < tower.attackCooldown)
                    continue;

                int closestIndex = FindClosestEnemy(tower);

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
            towersGradation[countTower] = visual.GetGradationTower();

            countTower++;
        }

        public void UpLevelTower(int towerIndex)
        {
            GradationTower gradationTower = towersGradation[towerIndex];
            ref TowerData tower = ref towers[towerIndex];
            int nextlevel = tower.level + 1;

            if (nextlevel >= gradationTower.levels.Length)
            {
                return;
            }

            LevelTower levelData = gradationTower.levels[nextlevel];

            tower.level = nextlevel;
            tower.attackRadius = levelData.attackRadius;
            tower.attackCooldown = levelData.attackCooldown;
            tower.damage = levelData.damage;
            tower.speed = levelData.speedProjectile;
            tower.targetMask = (int)levelData.targetTypes;

            towersVisual[towerIndex].SetVisualObject(levelData.obj);
        }

        public void RemoveTower(int towerIndex)
        {
            towers[towerIndex] = towers[countTower - 1];
            towersVisual[towerIndex] = towersVisual[countTower - 1];
            towersGradation[towerIndex] = towersGradation[countTower - 1];

            countTower--;
        }

        int FindClosestEnemy(TowerData tower)
        {
            int foundCount = gridManager.GetEnemiesInRadiusNonAlloc(
                tower.position,
                tower.attackRadius,
                enemySearchBuffer);

            int closestIndex = -1;
            float minDist = tower.attackRadius * tower.attackRadius;

            for (int i = 0; i < foundCount; i++)
            {
                int idx = enemySearchBuffer[i];

                if (idx >= enemyManager.countEnemies)
                    continue;

                if ((tower.targetMask & enemyManager.enemies[idx].types) == 0)
                    continue;

                ref EnemyData e = ref enemyManager.enemies[idx];

                float dist = (e.position - tower.position).sqrMagnitude;

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
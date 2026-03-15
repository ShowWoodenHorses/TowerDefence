using Assets.Scripts.Data;
using UnityEngine;
using Assets.Scripts.Configs;
using Assets.Scripts.Enum;
using System;
using Assets.Scripts.Visuals;

namespace Assets.Scripts.Managers
{
    public class TowerManager : MonoBehaviour
    {
        public TowerData[] towers;
        public TowerVisual[] towersVisuals;
        public GradationTower[] towersGradation;

        private int countTower;

        private EnemyManager enemyManager;
        private GridManager gridManager;
        private ProjectileManager projectileManager;
        private GameEvents gameEvents;

        private int[] enemySearchBuffer;

        public void Init(
            EnemyManager enemyManager,
            GridManager gridManager,
            ProjectileManager projectileManager,
            GameEvents gameEvents)
        {
            this.enemyManager = enemyManager;
            this.gridManager = gridManager;
            this.projectileManager = projectileManager;
            this.gameEvents = gameEvents;

            towers = new TowerData[64];
            towersVisuals = new TowerVisual[64];
            towersGradation = new GradationTower[64];
            enemySearchBuffer = new int[256];

            gameEvents.OnChangeMaskTargetTower.OnRaised += OnChangeMaskTarget;
            gameEvents.OnAddTowerVisual.OnRaised += OnAddTowerVisual;
        }

        private void Update()
        {
            float dt = Time.deltaTime;

            for (int i = 0; i < countTower; i++)
            {
                ref TowerData tower = ref towers[i];

                tower.timeSinceLastAttack += dt;

                if (tower.timeSinceLastAttack < tower.attackCooldown)
                    continue;

                LevelTower levelData = towersGradation[i].levels[tower.level];

                int maxTargets = levelData.targetingMode switch
                {
                    TargetingMode.Closest => 1,
                    TargetingMode.MultiTarget => levelData.maxTargets,
                    TargetingMode.AllInRadius => enemySearchBuffer.Length,
                    _ => 1
                };

                int foundTargets = FindEnemies(tower, maxTargets);

                if (foundTargets > 0)
                {
                    Shoot(tower, i, foundTargets);
                    tower.timeSinceLastAttack = 0f;
                }
            }
        }

        public int GetCurrentTowerIndex() => countTower;

        public void AddTower(TowerData data, LevelTower config, GradationTower gradationTower)
        {
            towers[countTower] = data;
            towersGradation[countTower] = gradationTower;

            gameEvents.OnCreateTower.Raise(countTower, config, data.position);

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
            tower.ApplyLevel(levelData);

            gameEvents.OnUpdateTower.Raise(towerIndex, levelData, tower.position);
        }

        public void RemoveTower(int towerIndex)
        {
            towers[towerIndex] = towers[countTower - 1];
            towersGradation[towerIndex] = towersGradation[countTower - 1];

            gameEvents.OnDeactivateTower.Raise(towerIndex);

            countTower--;
        }

        private int FindEnemies(TowerData tower, int maxTargets)
        {
            int foundCount = gridManager.GetEnemiesInRadiusNonAlloc(
                tower.position,
                tower.attackRadius,
                enemySearchBuffer);

            int validCount = 0;

            for (int i = 0; i < foundCount; i++)
            {
                int idx = enemySearchBuffer[i];

                if (idx >= enemyManager.countEnemies)
                    continue;

                if ((tower.targetMask & enemyManager.enemies[idx].types) == 0)
                    continue;

                enemySearchBuffer[validCount++] = idx;

                if (validCount >= maxTargets)
                    break;
            }

            return validCount;
        }

        private void Shoot(TowerData tower, int towerId, int targetCount)
        {
            TowerVisual visual = towersVisuals[towerId];
            if (visual == null)
                return;

            var gunPoints = visual.GunPoints;
            int guns = gunPoints.Length;

            for (int g = 0; g < guns; g++)
            {
                if (g >= targetCount)
                    break;

                int enemyIndex = enemySearchBuffer[g];

                Vector3 spawnPos = gunPoints[g].position;

                projectileManager.Spawn(
                    tower.projectileMask,
                    spawnPos,
                    enemyIndex,
                    tower.speed,
                    tower.damage
                );
            }
        }

        private void OnChangeMaskTarget(int id, ColorType newMask)
        {
            if (id >= 0 && id < towers.Length)
            {
                ref TowerData tower = ref towers[id];
                tower.targetMask = (int)newMask;
            }
        }

        private void OnAddTowerVisual(int id, TowerVisual visual)
        {
            towersVisuals[id] = visual;
        }

        private void OnDisable()
        {
            gameEvents.OnChangeMaskTargetTower.OnRaised -= OnChangeMaskTarget;
            gameEvents.OnAddTowerVisual.OnRaised -= OnAddTowerVisual;
        }
    }
}
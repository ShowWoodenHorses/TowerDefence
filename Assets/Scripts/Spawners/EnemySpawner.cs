using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Configs.Enemy;
using Assets.Scripts.Enum;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private int startCountEnemy;
        [SerializeField] private Transform startPos;
        [SerializeField] private float time;

        [SerializeField] private List<EnemyConfig> enemiesConfigs;
        [SerializeField] private List<EnemyTypeData> enemies;

        private EnemyManager enemyManager;
        private ColorTypeHandler colorTypeHandler;

        private List<int> availableTypes = new List<int>();

        public void Init(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;
            colorTypeHandler = new ColorTypeHandler();

            StartCoroutine(SpawnEnemy());
        }

        private IEnumerator SpawnEnemy()
        {
            var delay = new WaitForSeconds(time);

            int spawned = 0;

            while (spawned < startCountEnemy)
            {
                UpdateAvailableTypes();

                if (availableTypes.Count == 0)
                    yield break;

                int randomIndex = UnityEngine.Random.Range(0, availableTypes.Count);
                int typeIndex = availableTypes[randomIndex];

                EnemyTypeData enemyType = enemies[typeIndex];

                EnemyConfig config = GetConfig(enemyType.enemyType);
                ColorType colorType = colorTypeHandler.GetRandom();

                enemyManager.SpawnEnemy(
                    colorType,
                    colorTypeHandler,
                    config.enemyType,
                    startPos.position,
                    config.speed,
                    config.health
                );

                enemyType.CurrentCountEnemy++;
                enemies[typeIndex] = enemyType;

                spawned++;

                yield return delay;
            }
        }

        private void UpdateAvailableTypes()
        {
            availableTypes.Clear();

            for (int i = 0; i < enemies.Count; i++)
            {
                if (enemies[i].CurrentCountEnemy < enemies[i].GeneralCountEnemy)
                {
                    availableTypes.Add(i);
                }
            }
        }

        private EnemyConfig GetConfig(EnemyType type)
        {
            for (int i = 0; i < enemiesConfigs.Count; i++)
            {
                if (enemiesConfigs[i].enemyType == type)
                    return enemiesConfigs[i];
            }

            return null;
        }

        [Serializable]
        private struct EnemyTypeData
        {
            public int GeneralCountEnemy;
            public int CurrentCountEnemy;
            public EnemyType enemyType;
        }
    }
}
using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Pool;
using Assets.Scripts.Visuals;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class EnemyManager : MonoBehaviour
    {
        public List<Transform> points = new();
        public List<Vector3> pathPoints = new();

        public EnemyData[] enemies;
        public EnemyVisual[] visuals;

        public int countEnemies;

        private GridManager gridManager;
        private GameEvents gameEvents;
        private EnemyPool enemyPool;

        public void Init(GridManager grid, GameEvents gameEvents, EnemyPool enemyPool)
        {
            gridManager = grid;
            this.gameEvents = gameEvents;
            this.enemyPool = enemyPool;

            enemies = new EnemyData[512];
            visuals = new EnemyVisual[512];

            pathPoints.Clear();
            points.ForEach(p => pathPoints.Add(p.position));
        }

        void Update()
        {
            float dt = Time.deltaTime;

            // ИТЕРИРУЕМСЯ ТОЛЬКО ДО countEnemies
            for (int i = 0; i < countEnemies; i++)
            {
                ref EnemyData e = ref enemies[i];

                if (e.hp <= 0 || e.pointIndex >= pathPoints.Count)
                {
                    KillEnemy(i);
                    i--; // потому что мы сделали swap
                    continue;
                }

                Vector3 oldPos = e.position;
                var target = pathPoints[e.pointIndex];

                e.position = Vector3.MoveTowards(e.position, target, e.speed * dt);

                if ((e.position - target).sqrMagnitude < 0.01f)
                    e.pointIndex++;

                e.animOffset += dt * 2f;

                float tiltX = Mathf.Sin(e.animOffset * 0.5f) * 3f;
                float tiltZ = Mathf.Sin(e.animOffset) * 5f;

                visuals[i].transform.SetPositionAndRotation(
                    e.position,
                    Quaternion.Euler(tiltX, 0, tiltZ));

                gridManager.MoveEnemy(i, oldPos, e.position);
            }
        }

        public void SpawnEnemy(EnemyType type, Vector3 pos, float speed, int hp)
        {
            if (countEnemies >= enemies.Length)
            {
                Debug.LogError("Enemy array is full");
                return;
            }

            EnemyVisual v = enemyPool.Spawn(type, pos);

            if (v == null)
            {
                Debug.LogError("Spawn returned null");
                return;
            }

            enemies[countEnemies] = new EnemyData
            {
                position = pos,
                speed = speed,
                hp = hp,
                pointIndex = 0,
                animOffset = Random.Range(0f, Mathf.PI * 2),
            };
            enemies[countEnemies].version++;

            visuals[countEnemies] = v;

            gridManager.AddEnemy(countEnemies, pos);

            countEnemies++;
        }

        void KillEnemy(int index)
        {
            int lastIndex = countEnemies - 1;

            var deadEnemy = enemies[index];

            // Удаляем из грида
            gridManager.RemoveEnemy(index, deadEnemy.position);

            gameEvents.OnEnemyDied.OnRaised?.Invoke(visuals[index]);

            // Если не последний — делаем swap
            if (index != lastIndex)
            {
                enemies[index] = enemies[lastIndex];
                visuals[index] = visuals[lastIndex];

                // Обновляем грид для перемещённого врага
                gridManager.UpdateEnemyIndex(
                    lastIndex,
                    index,
                    enemies[index].position);
            }

            countEnemies--;
        }
    }
}
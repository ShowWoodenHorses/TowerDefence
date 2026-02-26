using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Managers;
using Assets.Scripts.Visuals;
using UnityEngine;

namespace Assets.Scripts.Spawners
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private int startCountEnemy;
        [SerializeField] private int startHealth;
        [SerializeField] private float startSpeed;
        [SerializeField] private Transform startPos;
        [SerializeField] private float time;
        [SerializeField] private List<GameObject> prefabs;

        private EnemyManager enemyManager;

        public void Init(EnemyManager enemyManager)
        {
            this.enemyManager = enemyManager;

            StartCoroutine(spawnEnemy());
        }
        private IEnumerator spawnEnemy()
        {
            var delay = new WaitForSeconds(time);

            for (int i = 0; i < startCountEnemy; i++)
            {
                foreach (var item in prefabs)
                {
                    yield return delay;
                    enemyManager.SpawnEnemy(item.GetComponent<EnemyVisual>().GetEnemyType(), startPos.position, startSpeed, startHealth);
                }
            }
        }
    }
}
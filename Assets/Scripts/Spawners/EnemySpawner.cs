using System.Collections;
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
        [SerializeField] private GameObject prefab;
        [SerializeField] private float time;

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
                yield return delay;
                enemyManager.SpawnEnemy(prefab.GetComponent<EnemyVisual>().GetEnemyType(), startPos.position, startSpeed, startHealth);
            }
        }
    }
}
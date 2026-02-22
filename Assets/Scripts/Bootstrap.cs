using Assets.Scripts.Configs;
using Assets.Scripts.Managers;
using Assets.Scripts.Pool;
using Assets.Scripts.Spawners;
using UnityEngine;

namespace Assets.Scripts
{
    public class Bootstrap : MonoBehaviour
    {
        [SerializeField] private EnemyManager enemyManager;
        [SerializeField] private TowerManager towerManager;
        [SerializeField] private EffectManager effectManager;
        [SerializeField] private ProjectileManager projectileManager;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private BuildManager buildManager;

        [SerializeField] private EnemySpawner enemySpawner;

        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private ProjectilePool projectilePool;

        [SerializeField] private GameEvents gameEvents;

        private void Awake()
        {
            gridManager.Init();

            effectManager.Init();
        }

        private void Start()
        {
            enemyPool.Init(gameEvents);
            projectilePool.Init(gameEvents);

            enemyManager.Init(gridManager, gameEvents, enemyPool);
            projectileManager.Init(enemyManager, gameEvents, projectilePool);
            towerManager.Init(enemyManager, gridManager, projectileManager);
            buildManager.Init(towerManager);
            enemySpawner.Init(enemyManager);
        }
    }
}
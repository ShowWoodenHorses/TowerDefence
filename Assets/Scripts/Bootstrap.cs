using Assets.Scripts.Building;
using Assets.Scripts.Configs;
using Assets.Scripts.Managers;
using Assets.Scripts.Pool;
using Assets.Scripts.Spawners;
using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.UI.Tower;

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
        [SerializeField] private TowerViewManager towerViewManager;

        [SerializeField] private EnemySpawner enemySpawner;

        [SerializeField] private EnemyPool enemyPool;
        [SerializeField] private ProjectilePool projectilePool;

        [SerializeField] private GameEvents gameEvents;
        [SerializeField] private List<TowerCell> towerCells;

        [Space, Header("UI")]
        [SerializeField] private SettingTowerViewPanel settingTowerViewPanel;
        [SerializeField] private PaletteViewPanel paletteViewPanel;

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
            towerManager.Init(enemyManager, gridManager, projectileManager, gameEvents);
            towerViewManager.Init(gameEvents);
            buildManager.Init(towerManager);
            enemySpawner.Init(enemyManager);
            paletteViewPanel.Init(gameEvents);
            settingTowerViewPanel.Init(gameEvents, paletteViewPanel);

            towerCells.ForEach(t => t.Init(settingTowerViewPanel));
        }
    }
}
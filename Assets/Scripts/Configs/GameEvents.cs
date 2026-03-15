using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameEvents", menuName = "Scriptable Objects/Game Events")]
    public class GameEvents : ScriptableObject
    {
        public EnemyEventChannel OnEnemyDied;
        public ProjectileEventChannel OnProjectileUsed;
        public CreateTowerEvent OnCreateTower;
        public CreateTowerEvent OnUpdateTower;
        public IntEventChannel OnDeactivateTower;
        public ChangeColorEvent OnChangeColorTower;
        public ChangeMaskTargetTowerEvent OnChangeMaskTargetTower;
        public TowerVisualEvent OnAddTowerVisual;
    }
}

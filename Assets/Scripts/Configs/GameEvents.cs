using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameEvents", menuName = "Scriptable Objects/Game Events")]
    public class GameEvents : ScriptableObject
    {
        public EnemyEventChannel OnEnemyDied;
        public ProjectileEventChannel OnProjectileUsed;
    }
}

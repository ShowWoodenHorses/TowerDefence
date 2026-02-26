using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Visuals
{
    public class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;

        public EnemyType GetEnemyType()
        {
            return enemyType;
        }
    }
}
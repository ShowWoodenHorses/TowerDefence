using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Visuals
{
    public class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private ColorType enemyType;

        public ColorType GetEnemyType()
        {
            return enemyType;
        }
    }
}
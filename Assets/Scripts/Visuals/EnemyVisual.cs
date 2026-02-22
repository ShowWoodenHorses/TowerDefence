using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Visuals
{
    public enum EnemyType
    {
        Box,
    }

    public class EnemyVisual : MonoBehaviour
    {
        [SerializeField] private EnemyType enemyType;

        public EnemyType GetEnemyType()
        {
            return enemyType;
        }
    }
}
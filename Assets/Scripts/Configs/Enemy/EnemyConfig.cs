using System.Collections;
using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Configs.Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Scriptable Objects/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        public ColorType ColorType;
        public GameObject prefab;
        public int health;
        public float speed;
    }
}
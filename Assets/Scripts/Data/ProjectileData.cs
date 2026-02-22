using System;
using Assets.Scripts.Visuals;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct ProjectileData
    {
        public Vector3 position;
        public Vector3 direction;
        public Vector3 targetPosition;

        public float speed;
        public int damage;

        public float remainingDistance;

        public ProjectileType type;

        public int targetIndex;
        public int targetVersion;
    }
}
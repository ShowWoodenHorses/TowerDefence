using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct EffectData
    {
        public Transform visual;
        public float duration;
        public float timer;
        public bool active;
    }
}
using System.Collections;
using Assets.Scripts.Configs;
using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Visuals
{
    public enum TowerType
    {
        Basic,
    }
    public class TowerVisual : MonoBehaviour
    {
        [SerializeField] private Transform[] gunPoints;

        public Transform[] GunPoints => gunPoints;
    }
}
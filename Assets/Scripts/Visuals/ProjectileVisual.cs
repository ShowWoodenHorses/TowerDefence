using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Visuals
{
    public enum ProjectileType
    {
        Dumb,
        Homing
    }
    public class ProjectileVisual : MonoBehaviour
    {
        [SerializeField] private ProjectileType type;

        public ProjectileType GetProjectileType() => type;
    }
}
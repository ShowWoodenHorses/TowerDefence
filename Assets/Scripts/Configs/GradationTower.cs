using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GradationTower", menuName = "Scriptable Objects/Gradation Tower")]
    public class GradationTower : ScriptableObject
    {
        public LevelTower[] levels;
    }
}
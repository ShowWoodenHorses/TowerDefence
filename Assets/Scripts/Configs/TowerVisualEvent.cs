using Assets.Scripts.Visuals;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "TowerVisualEvent", menuName = "Scriptable Objects/Tower Visual Event")]
    public class TowerVisualEvent : EventChannelTwoParametrsBase<int, TowerVisual>
    {
    }
}
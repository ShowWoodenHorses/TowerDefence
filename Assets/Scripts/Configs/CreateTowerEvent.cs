using Assets.Scripts.Data;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "CreateTowerEvent", menuName = "Scriptable Objects/Create Tower Event")]
    public class CreateTowerEvent : EventChannelThreeParametrsBase<int, GameObject, TowerData>
    {

    }
}
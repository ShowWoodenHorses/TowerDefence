using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ChangeMaskTargetTowerEvent", menuName = "Scriptable Objects/Change Mask Target Tower Event")]
    public class ChangeMaskTargetTowerEvent : EventChannelTwoParametrsBase<int, ColorType>
    {

    }
}
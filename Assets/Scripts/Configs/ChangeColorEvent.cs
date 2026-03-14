using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ChangeColorEvent", menuName = "Scriptable Objects/Change Color Event")]
    public class ChangeColorEvent : EventChannelTwoParametrsBase<int, Color>
    {

    }
}
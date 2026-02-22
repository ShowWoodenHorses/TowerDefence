using System.Collections;
using Assets.Scripts.Visuals;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "EnemyEventChannel", menuName = "Scriptable Objects/Enemy Event")]
    public class EnemyEventChannel : EventChannelBase<EnemyVisual>
    {

    }
}
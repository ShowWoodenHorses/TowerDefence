using System.Collections;
using Assets.Scripts.Visuals;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "ProjectileEventChannel", menuName = "Scriptable Objects/Projectile Event")]
    public class ProjectileEventChannel : EventChannelBase<ProjectileVisual>
    {

    }
}
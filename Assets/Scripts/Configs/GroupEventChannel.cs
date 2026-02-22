using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    [CreateAssetMenu(fileName = "GroupEventChannel", menuName = "Scriptable Objects/Group Event")]
    public class GroupEventChannel : ScriptableObject
    {
        public List<IntEventChannel> intEvents = new List<IntEventChannel>();
    }
}
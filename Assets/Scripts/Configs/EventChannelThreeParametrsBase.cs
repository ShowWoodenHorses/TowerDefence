using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    public abstract class EventChannelThreeParametrsBase<A, B, C> : ScriptableObject
    {
        public Action<A, B, C> OnRaised;

        public void Raise(A Avalue, B Bvalue, C Cvalue)
        {
            OnRaised?.Invoke(Avalue, Bvalue, Cvalue);
        }
    }
}
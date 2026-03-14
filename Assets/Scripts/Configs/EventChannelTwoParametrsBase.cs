using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    public abstract class EventChannelTwoParametrsBase<A, B> : ScriptableObject
    {
        public Action<A, B> OnRaised;

        public void Raise(A Avalue, B Bvalue)
        {
            OnRaised?.Invoke(Avalue, Bvalue);
        }
    }
}
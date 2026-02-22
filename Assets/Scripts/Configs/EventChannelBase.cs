using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Configs
{
    public abstract class EventChannelBase<T> : ScriptableObject
    {
        public Action<T> OnRaised;

        public void Raise(T value)
        {
            OnRaised?.Invoke(value);
        }
    }
}
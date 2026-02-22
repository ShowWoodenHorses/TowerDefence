using System.Collections.Generic;
using UnityEngine;

public abstract class PoolCore<T> : MonoBehaviour where T : Component
{
    protected class SubPool
    {
        public T prefab;
        public Stack<T> free = new();
        public HashSet<T> active = new();
    }

    protected Dictionary<int, SubPool> pools = new();

    protected T Create(SubPool p)
    {
        var obj = Instantiate(p.prefab);
        obj.gameObject.SetActive(false);
        return obj;
    }

    protected abstract void CreatePool();

    public T Spawn(int typeId, Vector3 pos, Quaternion rot)
    {
        if (!pools.TryGetValue(typeId, out var p))
        {
            Debug.LogError($"Pool type {typeId} not found");
            return null;
        }

        var obj = p.free.Count > 0 ? p.free.Pop() : Create(p);
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.gameObject.SetActive(true);
        p.active.Add(obj);
        return obj;
    }

    public void Despawn(int typeId, T obj)
    {
        if (!pools.TryGetValue(typeId, out var p)) return;
        if (!p.active.Remove(obj)) return;

        obj.gameObject.SetActive(false);
        p.free.Push(obj);
    }
}

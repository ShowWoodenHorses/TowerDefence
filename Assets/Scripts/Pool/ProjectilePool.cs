using Assets.Scripts.Configs;
using Assets.Scripts.Visuals;
using UnityEngine;
using System.Collections.Generic;
using System;

namespace Assets.Scripts.Pool
{
    public class ProjectilePool : PoolCore<ProjectileVisual>
    {
        [SerializeField] private List<ProjectileEntry> projectiles = new();

        private GameEvents gameEvents;

        public void Init(GameEvents events)
        {
            gameEvents = events;

            CreatePool();

            gameEvents.OnProjectileUsed.OnRaised += HandleReturn;
        }

        public ProjectileVisual Spawn(ProjectileType type, Vector3 pos)
        {
            return base.Spawn((int)type, pos, Quaternion.identity);
        }

        protected override void CreatePool()
        {
            foreach (var e in projectiles)
            {
                var sp = new SubPool { prefab = e.prefab };

                for (int i = 0; i < e.preload; i++)
                {
                    var v = Instantiate(e.prefab);
                    v.gameObject.SetActive(false);
                    sp.free.Push(v);
                }

                pools[(int)e.type] = sp;
            }
        }

        void HandleReturn(ProjectileVisual v)
        {
            base.Despawn((int)v.GetProjectileType(), v);
        }

        void OnDisable()
        {
            if (gameEvents != null)
                gameEvents.OnProjectileUsed.OnRaised -= HandleReturn;
        }
    }

    [Serializable]
    public class ProjectileEntry
    {
        public ProjectileType type;
        public ProjectileVisual prefab;
        public int preload = 10;
    }
}

using Assets.Scripts.Configs;
using Assets.Scripts.Visuals;
using UnityEngine;

namespace Assets.Scripts.Pool
{
    public class EnemyPool : PoolCore<EnemyVisual>
    {
        [SerializeField] private EnemyEntry[] entries;

        private GameEvents gameEvents;

        public void Init(GameEvents gameEvents)
        {
            this.gameEvents = gameEvents;

            CreatePool();

            gameEvents.OnEnemyDied.OnRaised += HandleReturn;
        }

        public EnemyVisual Spawn(EnemyType type, Vector3 pos)
        {

            if (!pools.ContainsKey((int)type))
            {
                Debug.LogError($"EnemyPool: type {type} not registered");
                return null;
            }

            var v = base.Spawn((int)type, pos, Quaternion.identity);
            return v;
        }

        protected override void CreatePool()
        {
            foreach (var e in entries)
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

        void OnDisable()
        {
            gameEvents.OnEnemyDied.OnRaised -= HandleReturn;
        }

        void HandleReturn(EnemyVisual visual)
        {
            base.Despawn((int)visual.GetEnemyType(), visual);
        }
    }


    [System.Serializable]
    public class EnemyEntry
    {
        public EnemyType type;
        public EnemyVisual prefab;
        public int preload = 10;
    }

}
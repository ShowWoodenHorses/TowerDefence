using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Data;

namespace Assets.Scripts.Managers
{
    public class EffectManager : MonoBehaviour
    {
        public Transform effectPrefab;
        public int poolSize = 20;

        private List<EffectData> effects = new();

        public void Init()
        {
            for (int i = 0; i < poolSize; i++)
            {
                Transform go = Instantiate(effectPrefab);
                go.gameObject.SetActive(false);
                effects.Add(new EffectData { visual = go, active = false });
            }
        }

        void Update()
        {
            float dt = Time.deltaTime;
            for (int i = 0; i < effects.Count; i++)
            {
                if (!effects[i].active) continue;

                var e = effects[i];
                e.timer += dt;
                if (e.timer >= e.duration)
                {
                    e.visual.gameObject.SetActive(false);
                    e.active = false;
                }
                effects[i] = e;
            }
        }

        public void PlayEffect(Vector3 position, float duration = 1f)
        {
            // ищем первый свободный эффект
            for (int i = 0; i < effects.Count; i++)
            {
                if (!effects[i].active)
                {
                    var e = effects[i];
                    e.visual.position = position;
                    e.visual.gameObject.SetActive(true);
                    e.duration = duration;
                    e.timer = 0f;
                    e.active = true;
                    effects[i] = e;
                    return;
                }
            }
        }
    }
}
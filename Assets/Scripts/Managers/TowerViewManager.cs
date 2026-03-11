using System.Collections;
using Assets.Scripts.Visuals;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Configs;
using Assets.Scripts.Data;
using System.Linq;

namespace Assets.Scripts.Managers
{
    public class TowerViewManager : MonoBehaviour
    {
        private Dictionary<int, TowerVisual> views = new();
        private List<GameObject> deactivateTowers = new();

        private GameEvents gameEvents;

        public void Init(GameEvents gameEvents)
        {
            this.gameEvents = gameEvents;

            gameEvents.OnCreateTower.OnRaised += SpawnView;
            gameEvents.OnUpdateTower.OnRaised += UpdateView;
            gameEvents.OnDeactivateTower.OnRaised += DeactivateView;
        }

        private void SpawnView(int id, GameObject prefab, Vector3 pos)
        {
            if (deactivateTowers.Contains(prefab))
            {
                prefab.transform.position = pos;
                views[id] = prefab.GetComponent<TowerVisual>();
                prefab.SetActive(true);
                return;
            }
            var v = Instantiate(prefab, pos, Quaternion.identity, transform);

            views[id] = v.GetComponent<TowerVisual>();
        }

        private void UpdateView(int id, GameObject prefab, Vector3 pos)
        {
            DeactivateView(id);

            SpawnView(id, prefab, pos);
        }

        private void DeactivateView(int id)
        {
            if (views.Keys.Contains(id))
            {
                TowerVisual visual = views[id];
                visual.gameObject.SetActive(false);
                deactivateTowers.Add(visual.gameObject);
            }
        }

        private void OnDestroy()
        {
            gameEvents.OnCreateTower.OnRaised -= SpawnView;
            gameEvents.OnUpdateTower.OnRaised -= UpdateView;
            gameEvents.OnDeactivateTower.OnRaised -= DeactivateView;
        }
    }
}
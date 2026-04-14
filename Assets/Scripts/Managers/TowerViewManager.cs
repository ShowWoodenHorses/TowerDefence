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
        [Header("Texture")]
        [SerializeField] private Texture2D gradientTexture;

        private Dictionary<int, TowerVisual> views = new();
        private List<GameObject> deactivateTowers = new();

        private GameEvents gameEvents;

        public void Init(GameEvents gameEvents)
        {
            this.gameEvents = gameEvents;

            gameEvents.OnCreateTower.OnRaised += SpawnView;
            gameEvents.OnUpdateTower.OnRaised += UpdateView;
            gameEvents.OnDeactivateTower.OnRaised += DeactivateView;
            gameEvents.OnChangeColorTower.OnRaised += ChangeColor;
        }

        private void SpawnView(int id, GameObject prefab, TowerData data)
        {
            if (deactivateTowers.Contains(prefab))
            {
                prefab.transform.position = data.position;
                views[id] = prefab.GetComponent<TowerVisual>();
                prefab.GetComponent<ColorSweep>().SetBaseColor(data.color);
                prefab.SetActive(true);
                return;
            }
            var v = Instantiate(prefab, data.position, Quaternion.identity, transform);
            v.GetComponent<ColorSweep>().SetBaseColor(data.color);

            views[id] = v.GetComponent<TowerVisual>();

            gameEvents.OnAddTowerVisual.Raise(id, v.GetComponent<TowerVisual>());
        }

        private void UpdateView(int id, GameObject prefab, TowerData data)
        {
            DeactivateView(id);

            SpawnView(id, prefab, data);
        }

        private void ChangeColor(int id, Color newColor)
        {
            if (views.Keys.Contains(id))
            {
                views[id].gameObject.GetComponent<ColorSweep>().ChangeColor(newColor, 1.5f);
            }
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
            gameEvents.OnChangeColorTower.OnRaised -= ChangeColor;
        }
    }
}
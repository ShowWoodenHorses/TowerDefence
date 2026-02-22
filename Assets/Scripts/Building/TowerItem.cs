using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Building
{
    public class TowerItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private GameObject towerPrefab;

        private Action<GameObject, Vector2> createTower;
        private GameObject ghost;

        public void Init(Action<GameObject, Vector2> createTower)
        {
            this.createTower = createTower;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            GhostTower(eventData.position);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            ghost.transform.position = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            ghost.SetActive(false);
            createTower?.Invoke(towerPrefab, eventData.position);
        }

        private void GhostTower(Vector3 startPosition)
        {
            if (ghost == null)
            {
                ghost = Instantiate(towerPrefab, startPosition, Quaternion.identity);
                return;
            }

            ghost.transform.position = startPosition;
            ghost.SetActive(true);
        }
    }
}
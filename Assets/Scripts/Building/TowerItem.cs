using System;
using Assets.Scripts.Configs;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.Building
{
    public class TowerItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        [SerializeField] private GradationTower gradationTower;

        private Action<GradationTower, Vector2> createTower;
        private GameObject ghost;

        public void Init(Action<GradationTower, Vector2> createTower)
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
            createTower?.Invoke(gradationTower, eventData.position);
        }

        private void GhostTower(Vector3 startPosition)
        {
            if (ghost == null)
            {
                ghost = Instantiate(gradationTower.levels[0].obj, startPosition, Quaternion.identity);
                return;
            }

            ghost.transform.position = startPosition;
            ghost.SetActive(true);
        }
    }
}
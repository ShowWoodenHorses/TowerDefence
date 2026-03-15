using System;
using Assets.Scripts.Configs;
using UnityEngine;

namespace Assets.Scripts.UI.Tower
{
    public class SettingTowerViewPanel : MonoBehaviour
    {
        [Header("UI")]
        [SerializeField] private RectTransform canvasRect;

        [Header("Offsets")]
        [SerializeField] private float worldOffsetY = 1.2f;
        [SerializeField] private float sideMargin = 20f;
        [SerializeField] private float verticalMargin = 20f;

        private RectTransform rect;
        private Camera cam;

        private GameEvents gameEvents;
        private PaletteViewPanel paletteViewPanel;

        private int currentTowerIndex;

        private bool isOpen = false;
        private bool isOpenPalette = false;

        private Transform target;

        private Action onTowerDestroy;

        public void Init(GameEvents gameEvents, PaletteViewPanel paletteViewPanel)
        {
            this.gameEvents = gameEvents;
            this.paletteViewPanel = paletteViewPanel;

            rect = GetComponent<RectTransform>();
            cam = Camera.main;

            Close();
        }

        private void LateUpdate()
        {
            if (!isOpen || target == null)
                return;

            UpdatePosition();
        }

        public void Open(int newIndex, Transform targetTransform, Action onTowerDestroy)
        {
            if (isOpen)
                return;

            isOpen = true;
            currentTowerIndex = newIndex;
            target = targetTransform;

            this.onTowerDestroy = onTowerDestroy;

            gameObject.SetActive(true);

            UpdatePosition();
        }

        public void Close()
        {
            isOpen = false;
            target = null;

            gameObject.SetActive(false);
        }

        public void OpenPalettePanel()
        {
            isOpenPalette = !isOpenPalette;

            if (isOpenPalette)
            {
                paletteViewPanel.Open(currentTowerIndex, transform.position);
                ToggleVisiblePanel(false);
            }
            else
            {
                ToggleVisiblePanel(true);
                paletteViewPanel.Close();
            }
        }

        public void UpLevel()
        {
            gameEvents.OnUpLevelTower.Raise(currentTowerIndex);
        }

        public void DestroyTower()
        {
            gameEvents.OnDeactivateTower.Raise(currentTowerIndex);
            onTowerDestroy?.Invoke();
            Close();
        }

        private void ToggleVisiblePanel(bool value)
        {
            gameObject.SetActive(value);
        }

        private void UpdatePosition()
        {
            Vector3 worldPos = target.position;
            worldPos.y += worldOffsetY;

            Vector2 screenPoint = cam.WorldToScreenPoint(worldPos);

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRect,
                screenPoint,
                null,
                out Vector2 canvasPoint);

            Vector2 panelSize = rect.rect.size;
            Vector2 canvasSize = canvasRect.rect.size;

            bool placeRight = canvasPoint.x < 0;
            bool placeTop = canvasPoint.y < 0;

            Vector2 finalPos = canvasPoint;

            if (placeRight)
                finalPos.x += panelSize.x * 0.5f + sideMargin;
            else
                finalPos.x -= panelSize.x * 0.5f + sideMargin;

            if (placeTop)
                finalPos.y += panelSize.y * 0.5f + verticalMargin;
            else
                finalPos.y -= panelSize.y * 0.5f + verticalMargin;

            finalPos = ClampToCanvas(finalPos);

            rect.localPosition = finalPos;
        }

        private Vector2 ClampToCanvas(Vector2 pos)
        {
            Vector2 panelSize = rect.rect.size;
            Vector2 canvasSize = canvasRect.rect.size;

            float x = Mathf.Clamp(
                pos.x,
                -canvasSize.x * 0.5f + panelSize.x * 0.5f,
                 canvasSize.x * 0.5f - panelSize.x * 0.5f);

            float y = Mathf.Clamp(
                pos.y,
                -canvasSize.y * 0.5f + panelSize.y * 0.5f,
                 canvasSize.y * 0.5f - panelSize.y * 0.5f);

            return new Vector2(x, y);
        }
    }
}
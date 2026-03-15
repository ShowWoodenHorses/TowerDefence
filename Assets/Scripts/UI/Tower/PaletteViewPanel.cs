using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Enum;
using UnityEngine;

namespace Assets.Scripts.UI.Tower
{
    public class PaletteViewPanel : MonoBehaviour
    {
        [SerializeField] private List<ColorViewItem> colorItems;
        private GameEvents gameEvents;
        private ColorTypeHandler colorTypeHandler;

        private int currentTowerIndex;
        private bool isOpen = false;
        public void Init(GameEvents gameEvents)
        {
            this.gameEvents = gameEvents;
            colorTypeHandler = new ColorTypeHandler();
            colorItems.ForEach(i => i.Init(OnSelectedColor));
            Close();
        }

        public void Open(int newIndex, Vector2 position)
        {
            if (isOpen)
                return;

            isOpen = true;
            currentTowerIndex = newIndex;
            transform.position = position;
            gameObject.SetActive(true);
            ShowColors();
        }
        public void Close()
        {
            isOpen = false;
            gameObject.SetActive(false);
        }
        public void HiddenColors()
        {
            colorItems.ForEach(i => i.gameObject.SetActive(false));
        }
        private void ShowColors()
        {
            colorItems.ForEach(i => i.gameObject.SetActive(true));
        }

        private void OnSelectedColor(ColorType color)
        {
            gameEvents.OnChangeColorTower.Raise(currentTowerIndex, colorTypeHandler.GetColor(color));
            gameEvents.OnChangeMaskTargetTower.Raise(currentTowerIndex, color);
        }
    }
}
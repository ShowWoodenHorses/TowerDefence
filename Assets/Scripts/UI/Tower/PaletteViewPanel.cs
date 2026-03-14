using System;
using System.Collections.Generic;
using Assets.Scripts.Configs;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

namespace Assets.Scripts.UI.Tower
{
    public enum ColorTower
    {
        None,
        Black,
        White,
        Blue,
        Red,
        Green,
        Yellow,
        Purple,
        Orange,
    }
    public class PaletteViewPanel : MonoBehaviour
    {
        [SerializeField] private List<ColorViewItem> colorItems;
        private GameEvents gameEvents;

        private int currentTowerIndex;
        private bool isOpen = false;
        public void Init(GameEvents gameEvents)
        {
            this.gameEvents = gameEvents;
            colorItems.ForEach(i => i.Init(OnSelectedColor));
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

        private void OnSelectedColor(ColorTower color)
        {
            gameEvents.OnChangeColorTower.Raise(currentTowerIndex, GetColor(color));
        }

        private Color GetColor(ColorTower color)
        {
            switch (color)
            {
                case ColorTower.Black:
                    return Color.black;
                case ColorTower.Blue:
                    return Color.blue;
                case ColorTower.Red:
                    return Color.red;
                case ColorTower.Green:
                    return Color.green;
                case ColorTower.Yellow:
                    return Color.yellow;
                case ColorTower.Purple:
                    return Color.purple;
                case ColorTower.Orange:
                    return Color.orange;
                default:
                    return Color.white;
            }
        }
    }
}
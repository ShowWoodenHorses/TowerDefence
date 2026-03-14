using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Tower
{
    public class ColorViewItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ColorTower colorTower;
        private Action<ColorTower> onSelectColor; 
        public void Init(Action<ColorTower> onSelectColor)
        {
            this.onSelectColor = onSelectColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onSelectColor?.Invoke(colorTower);
        }
    }
}
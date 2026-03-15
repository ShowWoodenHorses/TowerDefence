using System;
using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.UI.Tower
{
    public class ColorViewItem : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private ColorType ColorType;
        private Action<ColorType> onSelectColor; 
        public void Init(Action<ColorType> onSelectColor)
        {
            this.onSelectColor = onSelectColor;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            onSelectColor?.Invoke(ColorType);
        }
    }
}
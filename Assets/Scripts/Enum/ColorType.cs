using System;
using UnityEngine;

namespace Assets.Scripts.Enum
{
    [Flags]
    public enum ColorType
    {
        None = 0,
        Black = 1 << 1,
        White = 1 << 2,
        Blue = 1 << 3, 
        Green = 1 << 4,
        Red = 1 << 5,
        Yellow = 1 << 6,
        Purple = 1 << 7,
        Orange = 1 << 8,
    }

    public class ColorTypeHandler
    {
        public ColorType[] cachedValues;

        public ColorTypeHandler()
        {
            cachedValues = Array.FindAll(
                (ColorType[])System.Enum.GetValues(typeof(ColorType)),
                c => c != ColorType.None
            );
        }
        public Color GetColor(ColorType color)
        {
            switch (color)
            {
                case ColorType.Black:
                    return Color.black;
                case ColorType.Blue:
                    return Color.blue;
                case ColorType.Red:
                    return Color.red;
                case ColorType.Green:
                    return Color.green;
                case ColorType.Yellow:
                    return Color.yellow;
                case ColorType.Purple:
                    return Color.purple;
                case ColorType.Orange:
                    return Color.orange;
                default:
                    return Color.white;
            }
        }

        public ColorType GetRandom()
        {
            return cachedValues[UnityEngine.Random.Range(0, cachedValues.Length)];
        }

    }
}
using System;

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
}
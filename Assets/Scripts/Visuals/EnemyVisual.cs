using Assets.Scripts.Enum;
using UnityEngine;
using UnityEngine.Serialization;

namespace Assets.Scripts.Visuals
{
    [RequireComponent(typeof(ColorSweep))]
    public class EnemyVisual : MonoBehaviour
    {
        [FormerlySerializedAs("enemyType")]
        [SerializeField] private ColorType colorType;
        [SerializeField] private EnemyType enemyType;

        private ColorSweep colorSweep;

        public ColorType GetColorType()
        {
            return colorType;
        }
        public EnemyType GetEnemyType()
        {
            return enemyType;
        }

        private void Awake()
        {
            colorSweep = GetComponent<ColorSweep>();
        }

        public void SetColor(ColorType newcolorType, ColorTypeHandler handler)
        {
            colorType = newcolorType;
            Color color = handler.GetColor(colorType);
            colorSweep.SetBaseColor(color);
        }
    }
}
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Animation
{
    public class TweenController : MonoBehaviour
    {
        public enum MotionType
        {
            None,

            // Вращение
            RotateHorizontal,   // вокруг Y
            RotateVertical,     // вокруг X
            RotateForward,      // вокруг Z

            // Движение
            MoveUpDown,
            MoveLeftRight,
            MoveForwardBack
        }

        [Header("Settings")]
        [SerializeField] private MotionType motionType = MotionType.None;
        [SerializeField] private Transform target;
        [SerializeField] private float speed = 1f;
        [SerializeField] private float amplitude = 1f;
        private Vector3 startPos;

        void Start()
        {
            if (target == null)
                return;

            startPos = target.position;
        }

        void Update()
        {
            if (target == null)
                return;

            float value = Mathf.Sin(Time.time * speed) * amplitude;

            switch (motionType)
            {
                // 🔄 Вращение
                case MotionType.RotateHorizontal:
                    target.Rotate(Vector3.up * speed * Time.deltaTime);
                    break;

                case MotionType.RotateVertical:
                    target.Rotate(Vector3.right * speed * Time.deltaTime);
                    break;

                case MotionType.RotateForward:
                    target.Rotate(Vector3.forward * speed * Time.deltaTime);
                    break;

                // ↕ Движение
                case MotionType.MoveUpDown:
                    target.position = startPos + Vector3.up * value;
                    break;

                case MotionType.MoveLeftRight:
                    target.position = startPos + Vector3.right * value;
                    break;

                case MotionType.MoveForwardBack:
                    target.position = startPos + Vector3.forward * value;
                    break;
            }
        }
    }
}
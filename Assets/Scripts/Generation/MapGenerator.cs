using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Assets.Scripts.Generation
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("Path settings")]
        public List<Transform> pathPoints;
        public GameObject roadPrefab;
        public float stepDistance = 2f;

        [Header("Map settings")]
        public Vector2 mapSize = new Vector2(50, 50);
        public GameObject[] environmentPrefabs;
        public float environmentSpacing = 2f;
        public Vector3 environmentOffset = Vector3.zero;

        [Header("Parents")]
        public Transform roadParent;
        public Transform environmentParent;

        private List<Vector3> roadPositions = new List<Vector3>();

        public void GenerateRoad()
        {
            ClearRoad();

            roadPositions.Clear();

            for (int i = 0; i < pathPoints.Count - 1; i++)
            {
                Vector3 start = pathPoints[i].position;
                Vector3 end = pathPoints[i + 1].position;

                float distance = Vector3.Distance(start, end);
                int steps = Mathf.CeilToInt(distance / stepDistance);

                for (int j = 0; j <= steps; j++)
                {
                    float t = j / (float)steps;
                    Vector3 pos = Vector3.Lerp(start, end, t);

                    GameObject obj = Instantiate(roadPrefab, pos, Quaternion.identity, roadParent);

                    // Поворот дороги по направлению
                    Vector3 dir = (end - start).normalized;
                    if (dir != Vector3.zero)
                        obj.transform.rotation = Quaternion.LookRotation(dir);

                    roadPositions.Add(pos);
                }
            }
        }
        public void GenerateEnvironment()
        {
            ClearEnvironment();

            for (float x = -mapSize.x / 2; x < mapSize.x / 2; x += environmentSpacing)
            {
                for (float z = -mapSize.y / 2; z < mapSize.y / 2; z += environmentSpacing)
                {
                    Vector3 pos = transform.position + new Vector3(x, 0, z) + environmentOffset;

                    if (IsNearRoad(pos)) continue;

                    GameObject prefab = environmentPrefabs[Random.Range(0, environmentPrefabs.Length)];

                    Instantiate(prefab, pos, Quaternion.Euler(0, RandomRotate(), 0), environmentParent);
                }
            }
        }

        // Проверка — рядом ли дорога
        private bool IsNearRoad(Vector3 pos)
        {
            foreach (var roadPos in roadPositions)
            {
                if (Vector3.Distance(pos, roadPos) < stepDistance)
                    return true;
            }
            return false;
        }
        public void ClearAll()
        {
            ClearRoad();
            ClearEnvironment();
        }

        private void ClearRoad()
        {
            if (roadParent == null) return;

            for (int i = roadParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(roadParent.GetChild(i).gameObject);
            }
        }
        private void ClearEnvironment()
        {
            if (environmentParent == null) return;

            for (int i = environmentParent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(environmentParent.GetChild(i).gameObject);
            }
        }

        private float RandomRotate()
        {
            float[] rotations = new float[2] {0f, 90f};
            return rotations[Random.Range(0, rotations.Length)];
        }

#if UNITY_EDITOR
        [CustomEditor(typeof(MapGenerator))]
        public class MapGeneratorEditor : Editor
        {
            public override void OnInspectorGUI()
            {
                DrawDefaultInspector();

                MapGenerator gen = (MapGenerator)target;

                GUILayout.Space(10);

                if (GUILayout.Button("🚧 Generate Road"))
                {
                    gen.GenerateRoad();
                }

                if (GUILayout.Button("🌳 Generate Environment"))
                {
                    gen.GenerateEnvironment();
                }

                if (GUILayout.Button("🧹 Clear Environment"))
                {
                    gen.ClearEnvironment();
                }

                if (GUILayout.Button("🧹 Clear ALL"))
                {
                    gen.ClearAll();
                }
            }
        }
#endif
    }
}
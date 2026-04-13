using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Generation
{
    public class MapGenerator : MonoBehaviour
    {
        [Header("===== ROAD =====")]
        public List<Transform> pathPoints;
        public GameObject roadPrefab;
        public GameObject roadTurnPrefab;
        public float stepDistance = 2f;

        [Header("===== MOUNTAINS =====")]
        public List<Transform> mountainPoints;
        public GameObject[] mountainPrefabs;

        [Header("===== RIVER =====")]
        public List<Transform> riverPoints;
        public GameObject riverPrefab;

        [Header("===== TOWER PLACES =====")]
        public List<Transform> towerPoints;
        public GameObject towerPlacePrefab;

        [Header("===== MAP =====")]
        public Vector2 mapSize = new Vector2(50, 50);
        public GameObject[] environmentPrefabs;
        public float environmentSpacing = 2f;
        public Vector3 environmentOffset = Vector3.zero;

        [Header("===== PARENTS =====")]
        public Transform roadParent;
        public Transform environmentParent;
        public Transform mountainParent;
        public Transform riverParent;
        public Transform towerParent;

        // 🔥 единый список занятых мест
        private List<Vector3> blockedPositions = new List<Vector3>();

        // =====================================================
        // 🚧 ROAD
        // =====================================================
        public void GenerateRoad()
        {
            ClearChildren(roadParent);
            blockedPositions.Clear();
            if (pathPoints.Count < 2 || roadPrefab == null) return;

            for (int i = 0; i < pathPoints.Count - 1; i++)
            {
                Vector3 start = pathPoints[i].position;
                Vector3 end = pathPoints[i + 1].position;

                float distance = Vector3.Distance(start, end);
                int steps = Mathf.CeilToInt(distance / stepDistance);

                for (int j = 0; j <= steps; j++)
                {
                    // ❗ ПРОПУСКАЕМ последнюю точку сегмента
                    // чтобы не было дублей
                    if (j == steps && i < pathPoints.Count - 2)
                        continue;

                    float t = j / (float)steps;
                    Vector3 pos = Vector3.Lerp(start, end, t);

                    bool isTurnPoint = (j == 0 && i > 0);

                    GameObject prefabToSpawn = roadPrefab;

                    // 🎯 если это точка поворота
                    if (isTurnPoint && roadTurnPrefab != null)
                    {
                        prefabToSpawn = roadTurnPrefab;
                    }

                    GameObject obj = Instantiate(prefabToSpawn, pos, Quaternion.identity, roadParent);

                    // Вращение только для обычной дороги
                    if (!isTurnPoint)
                    {
                        Vector3 dir = (end - start).normalized;
                        if (dir != Vector3.zero)
                            obj.transform.rotation = Quaternion.LookRotation(dir);
                    }

                    blockedPositions.Add(pos);
                }
            }
        }

        // =====================================================
        // 🌊 RIVER
        // =====================================================
        public void GenerateRiver()
        {
            ClearChildren(riverParent);

            foreach (var point in riverPoints)
            {
                GameObject prefab = riverPrefab;

                Vector3 pos = point.position;
                Instantiate(prefab, pos, Quaternion.Euler(0, RandomRotate(), 0), riverParent);

                blockedPositions.Add(pos);
            }
        }

        // =====================================================
        // ⛰ MOUNTAINS
        // =====================================================
        public void GenerateMountains()
        {
            ClearChildren(mountainParent);

            foreach (var point in mountainPoints)
            {
                GameObject prefab = mountainPrefabs[Random.Range(0, mountainPrefabs.Length)];

                Vector3 pos = point.position;
                Instantiate(prefab, pos, Quaternion.Euler(0, RandomRotate(), 0), mountainParent);

                blockedPositions.Add(pos);
            }
        }

        // =====================================================
        // 🏰 TOWER PLACES
        // =====================================================
        public void GenerateTowerPlaces()
        {
            ClearChildren(towerParent);

            foreach (var point in towerPoints)
            {
                Vector3 pos = point.position;

                Instantiate(towerPlacePrefab, pos, Quaternion.identity, towerParent);

                blockedPositions.Add(pos);
            }
        }

        // =====================================================
        // 🌳 ENVIRONMENT
        // =====================================================
        public void GenerateEnvironment()
        {
            ClearChildren(environmentParent);

            for (float x = -mapSize.x / 2; x < mapSize.x / 2; x += environmentSpacing)
            {
                for (float z = -mapSize.y / 2; z < mapSize.y / 2; z += environmentSpacing)
                {
                    Vector3 pos = transform.position + new Vector3(x, 0, z) + environmentOffset;

                    if (IsBlocked(pos)) continue;

                    GameObject prefab = environmentPrefabs[Random.Range(0, environmentPrefabs.Length)];

                    Instantiate(prefab, pos, Quaternion.Euler(0, RandomRotate(), 0), environmentParent);
                }
            }
        }

        // =====================================================
        // 🚫 ПРОВЕРКА ЗАНЯТОСТИ
        // =====================================================
        private bool IsBlocked(Vector3 pos)
        {
            foreach (var b in blockedPositions)
            {
                if (Vector3.Distance(pos, b) < stepDistance)
                    return true;
            }
            return false;
        }

        // =====================================================
        // 🧹 CLEAR
        // =====================================================
        public void ClearAll()
        {
            ClearChildren(roadParent);
            ClearChildren(environmentParent);
            ClearChildren(mountainParent);
            ClearChildren(riverParent);
            ClearChildren(towerParent);

            blockedPositions.Clear();
        }

        private void ClearChildren(Transform parent)
        {
            if (parent == null) return;

            for (int i = parent.childCount - 1; i >= 0; i--)
            {
                DestroyImmediate(parent.GetChild(i).gameObject);
            }
        }

        private float RandomRotate()
        {
            float[] rotations = new float[2] { 0f, 90f };
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
                    gen.GenerateRoad();

                if (GUILayout.Button("🌊 Generate River"))
                    gen.GenerateRiver();

                if (GUILayout.Button("⛰ Generate Mountains"))
                    gen.GenerateMountains();

                if (GUILayout.Button("🏰 Generate Tower Places"))
                    gen.GenerateTowerPlaces();

                if (GUILayout.Button("🌳 Generate Environment"))
                    gen.GenerateEnvironment();

                if (GUILayout.Button("🧹 Clear ALL"))
                    gen.ClearAll();
            }
        }
#endif
    }
}
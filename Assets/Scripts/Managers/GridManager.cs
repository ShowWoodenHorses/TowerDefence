using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Managers
{
    public class GridManager : MonoBehaviour
    {
        public int gridWidth = 20;
        public int gridHeight = 20;
        public float cellSize = 2f;

        public List<int>[,] grid; // теперь храним ID врагов

        public void Init()
        {
            grid = new List<int>[gridWidth, gridHeight];
            for (int x = 0; x < gridWidth; x++)
                for (int z = 0; z < gridHeight; z++)
                    grid[x, z] = new List<int>();
        }

        public Vector2Int WorldToCell(Vector3 pos)
        {
            int x = Mathf.Clamp((int)(pos.x / cellSize), 0, gridWidth - 1);
            int z = Mathf.Clamp((int)(pos.z / cellSize), 0, gridHeight - 1);
            return new Vector2Int(x, z);
        }

        public void AddEnemy(int id, Vector3 pos)
        {
            var cell = WorldToCell(pos);
            grid[cell.x, cell.y].Add(id);
        }

        public void MoveEnemy(int id, Vector3 oldPos, Vector3 newPos)
        {
            var oldCell = WorldToCell(oldPos);
            var newCell = WorldToCell(newPos);

            if (oldCell != newCell)
            {
                grid[oldCell.x, oldCell.y].Remove(id);
                grid[newCell.x, newCell.y].Add(id);
            }
        }

        public void RemoveEnemy(int id, Vector3 pos)
        {
            var cell = WorldToCell(pos);
            grid[cell.x, cell.y].Remove(id);
        }

        public int GetEnemiesInRadiusNonAlloc(
            Vector3 pos,
            float radius,
            int[] resultsBuffer)
        {
            int count = 0;

            int cellRadius = Mathf.CeilToInt(radius / cellSize);
            var center = WorldToCell(pos);

            for (int x = center.x - cellRadius; x <= center.x + cellRadius; x++)
            {
                for (int z = center.y - cellRadius; z <= center.y + cellRadius; z++)
                {
                    if (x < 0 || x >= gridWidth || z < 0 || z >= gridHeight)
                        continue;

                    var cellList = grid[x, z];

                    for (int i = 0; i < cellList.Count; i++)
                    {
                        if (count >= resultsBuffer.Length)
                            return count;

                        resultsBuffer[count++] = cellList[i];
                    }
                }
            }

            return count;
        }

        public void UpdateEnemyIndex(int oldIndex, int newIndex, Vector3 pos)
        {
            var cell = WorldToCell(pos);

            var list = grid[cell.x, cell.y];

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == oldIndex)
                {
                    list[i] = newIndex;
                    return;
                }
            }
        }
    }
}

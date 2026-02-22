using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public class TowerCell : MonoBehaviour
    {
        private int towerIndex;
        private bool hasTower;

        public int GetTowerIndex() => towerIndex;
        public Vector3 GetPosition() => transform.position;
        public bool HasTower() => hasTower;

        public void BuildTower(int newIndex)
        {
            towerIndex = newIndex;
            hasTower = true;
        }

        public void DestroyTower()
        {
            hasTower = false;
        }

    }
}
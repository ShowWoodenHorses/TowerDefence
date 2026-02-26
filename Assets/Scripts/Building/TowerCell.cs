using System.Collections.Generic;
using Assets.Scripts.Configs;
using Assets.Scripts.Data;
using Assets.Scripts.Managers;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public class TowerCell : MonoBehaviour
    {
        [SerializeField] private Material m_invisible;
        private Material[] startMaterials;
        private Material[] currentMaterials;
        private int towerIndex;
        private bool hasTower;

        private TowerManager towerManager;

        public void Init(TowerManager towerManager)
        {
            this.towerManager = towerManager;
        }

        public int GetTowerIndex() => towerIndex;
        public Vector3 GetPosition() => transform.position;
        public bool HasTower() => hasTower;

        public void BuildTower(int newIndex)
        {
            towerIndex = newIndex;
            hasTower = true;

            startMaterials = GetComponent<MeshRenderer>().materials;
            currentMaterials = startMaterials;

            for (int i = 0; i < currentMaterials.Length; i++)
            {
                currentMaterials[i] = m_invisible;
            }
        }

        public void UpdateTower()
        {
            towerManager.UpLevelTower(towerIndex);
        }

        public void DestroyTower()
        {
            hasTower = false;

            for (int i = 0; i < startMaterials.Length; i++)
            {
                currentMaterials[i] = startMaterials[i];
            }
        }

    }
}
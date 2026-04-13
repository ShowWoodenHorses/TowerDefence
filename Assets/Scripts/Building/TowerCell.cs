using Assets.Scripts.UI.Tower;
using UnityEngine;

namespace Assets.Scripts.Building
{
    public class TowerCell : MonoBehaviour
    {
        [SerializeField] private Vector3 offsetPosition;
        private int towerIndex;
        private bool hasTower;

        private SettingTowerViewPanel settingTowerViewPanel;

        public void Init(SettingTowerViewPanel settingTowerViewPanel)
        {
            this.settingTowerViewPanel = settingTowerViewPanel;
        }

        public int GetTowerIndex() => towerIndex;
        public Vector3 GetPosition() => transform.position + offsetPosition;
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

        public void ShowSettingTowerPanel()
        {
            settingTowerViewPanel.Open(towerIndex, transform, DestroyTower);
        }

        private void OnMouseDown()
        {
            if (!hasTower)
                return;

            ShowSettingTowerPanel();
        }

    }
}
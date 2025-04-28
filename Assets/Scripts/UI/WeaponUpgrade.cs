using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponUpgrade : MonoBehaviour
{
    public SaveData _SaveData { get; private set; }

    [SerializeField] private GameObject weaponUpgradePanel;
    [SerializeField] WeaponUpgradeButtons[] upgradeButtons;
    //List<WeaponData> _weapons;
    private void Awake()
    {
        weaponUpgradePanel.SetActive(false);
    }
    private void Start()
    {
        //_weapons = GameDataService.Instance.Data.weapons;
        
    }
    public void GetData()
    {
        _SaveData = LevelManager.Instance._SaveData;
    }

    public void OpenWeaponUpgradePanel(List<WeaponData>  _weapons)
    {
        weaponUpgradePanel.SetActive(true);
        GetData();
        foreach(var weapon in _weapons)
        {
            foreach (var upgrade in upgradeButtons) { 
                if(weapon.Type == upgrade.Type.ToString())
                {
                    upgrade.weaponUpgrade = this;
                    upgrade.GetWeaponUpgrateDetailes(weapon);
                }
            }
        }
        //for (int i = 0; i < upgradeButtons.Length; i++)
        //{
        //    upgradeButtons[i].GetWeaponUpgrateDetailes(_weapons[i]);
        //}
    }
    public void UpgradeorUnlockClick(WeaponData _weaponData)
    {
        //weaponUpgradePanel.SetActive(false);
        //WaveManager.Instance.StartWavesAfterUpgrade();
        GameDataService.Instance.UpdateWeaponData(_weaponData);
        OpenWeaponUpgradePanel(GameDataService.Instance.GetWeaponData());
        ActiveBlocks.GetOpenedBlocks();

    }
    public void ExitWeaponUpgradePanel()
    {
        Debug.Log("Exit Weapon Upgrade Panel");
        weaponUpgradePanel.SetActive(false);
        WaveManager.Instance.StartWavesAfterUpgrade();
    }
}

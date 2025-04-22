using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Prefab and Parents")]
    public GameObject UpgradeUIPrefab;
    public RectTransform [] UpgradeUIParents;

    private Dictionary<UpgradeType, UpgradeData> upgradeLookup;

    [SerializeField] private GameObject UpgradePanel;
    public void OnEnableUpgradeScene()
    {
        UpgradePanel.SetActive(true);
        InitializeUpgradeData();
        GenerateRandomUpgrades();
    }
    
    private void InitializeUpgradeData()
    {
        upgradeLookup = new Dictionary<UpgradeType, UpgradeData>
        {
            { UpgradeType.HealthPlus20, new UpgradeData { Type = UpgradeType.HealthPlus20, Description = "+20% Health", CrystalCost = 55 } },
            { UpgradeType.BallSpeedPlus20, new UpgradeData { Type = UpgradeType.BallSpeedPlus20, Description = "+20% Ball Speed", CrystalCost = 100 } },
            { UpgradeType.AllWeaponsHealthPlus10, new UpgradeData { Type = UpgradeType.AllWeaponsHealthPlus10, Description = "+10% All Weapons Health", CrystalCost = 125 } },
            { UpgradeType.CoinPlus50, new UpgradeData { Type = UpgradeType.CoinPlus50, Description = "+50 Coins", CrystalCost = 20 } },
            { UpgradeType.BallAccelerationPlus10, new UpgradeData { Type = UpgradeType.BallAccelerationPlus10, Description = "+10% Ball Acceleration", CrystalCost = 50 } },
            { UpgradeType.ExtraBall, new UpgradeData { Type = UpgradeType.ExtraBall, Description = "+1 Extra Ball", CrystalCost = 80 } },
            { UpgradeType.RandomWeaponHealthPlus20, new UpgradeData { Type = UpgradeType.RandomWeaponHealthPlus20, Description = "Random Weapon +20% Health", CrystalCost = 40 } },
            { UpgradeType.WeaponDamagePlus15, new UpgradeData { Type = UpgradeType.WeaponDamagePlus15, Description = "+15% Weapon Damage", CrystalCost = 75 } },
            { UpgradeType.TetrisHealthPlus25, new UpgradeData { Type = UpgradeType.TetrisHealthPlus25, Description = "+25% Tetris Block Health", CrystalCost = 165 } },
            { UpgradeType.ZombieDamageMinus10, new UpgradeData { Type = UpgradeType.ZombieDamageMinus10, Description = "-10% Damage from Zombies", CrystalCost = 55 } },
            { UpgradeType.DropRandomTetris, new UpgradeData { Type = UpgradeType.DropRandomTetris, Description = "Drop Random Tetris Block", CrystalCost = 40 } },
            { UpgradeType.CoinPlus100, new UpgradeData { Type = UpgradeType.CoinPlus100, Description = "+100 Coins", CrystalCost = 35 } },
            { UpgradeType.ZombiesSlower5Percent, new UpgradeData { Type = UpgradeType.ZombiesSlower5Percent, Description = "Zombies 5% Slower", CrystalCost = 60 } },
            { UpgradeType.CoinGainPlus20Percent, new UpgradeData { Type = UpgradeType.CoinGainPlus20Percent, Description = "+20% Coin Gain", CrystalCost = 45 } },
            { UpgradeType.WallHealthPlus30, new UpgradeData { Type = UpgradeType.WallHealthPlus30, Description = "+30% Wall Health", CrystalCost = 50 } },
            { UpgradeType.TetrisBlockHealthPlus1, new UpgradeData { Type = UpgradeType.TetrisBlockHealthPlus1, Description = "+1 Tetris Block Health", CrystalCost = 35 } },
            { UpgradeType.RandomBlockRepair, new UpgradeData { Type = UpgradeType.RandomBlockRepair, Description = "+1 Random Block Repair", CrystalCost = 40 } },
            { UpgradeType.CritChancePlus10, new UpgradeData { Type = UpgradeType.CritChancePlus10, Description = "+10% Crit Chance", CrystalCost = 90 } },
            { UpgradeType.BallPierceTwoEnemies, new UpgradeData { Type = UpgradeType.BallPierceTwoEnemies, Description = "Balls Pierce 2 Enemies", CrystalCost = 80 } },
            { UpgradeType.CoinPlus2PerKill, new UpgradeData { Type = UpgradeType.CoinPlus2PerKill, Description = "+2 Coins per Kill", CrystalCost = 60 } },
            { UpgradeType.ExtraWeaponSlot, new UpgradeData { Type = UpgradeType.ExtraWeaponSlot, Description = "+1 Weapon Slot", CrystalCost = 100 } },
            { UpgradeType.ZombiesFaster10CoinX2, new UpgradeData { Type = UpgradeType.ZombiesFaster10CoinX2, Description = "Zombies +10% Faster, x2 Coins", CrystalCost = 50 } },
        };
    }

    public void GenerateRandomUpgrades()
    {
        HashSet<UpgradeType> selectedTypes = new HashSet<UpgradeType>();
        var types = new List<UpgradeType>(upgradeLookup.Keys);

        while (selectedTypes.Count < 3)
        {
            var random = types[Random.Range(0, types.Count)];
            selectedTypes.Add(random);
        }

        int count = 0;
        foreach (UpgradeType type in selectedTypes)
        {
            UpgradeData data = upgradeLookup[type];
            Transform parent = UpgradeUIParents.Length > count ? UpgradeUIParents[count] : UpgradeUIParents[0];

            // Instantiate the prefab
            GameObject obj = Instantiate(UpgradeUIPrefab, parent);

            // Get the controller script from the instantiated prefab
            UpgradeUIPrefabController controller = obj.GetComponent<UpgradeUIPrefabController>();
            controller.gameObject = UpgradePanel;
            if (controller != null)
            {
                // Set the text and button action using the new method
                controller.SetUpgradeData(data.Description, data.CrystalCost, () => OnUpgradeSelected(data.Type));
            }
            else
            {
                Debug.LogError("UpgradeUIPrefabController script is missing on prefab.");
            }

            count++;
        }
    }


    private void OnUpgradeSelected(UpgradeType type)
    {
        switch (type)
        {
            case UpgradeType.HealthPlus20: ApplyHealthPlus20(); break;
            case UpgradeType.BallSpeedPlus20: ApplyBallSpeedPlus20(); break;
            case UpgradeType.AllWeaponsHealthPlus10: ApplyAllWeaponsHealthPlus10(); break;
            case UpgradeType.CoinPlus50: ApplyCoinPlus50(); break;
            case UpgradeType.BallAccelerationPlus10: ApplyBallAccelerationPlus10(); break;
            case UpgradeType.ExtraBall: ApplyExtraBall(); break;
            case UpgradeType.RandomWeaponHealthPlus20: ApplyRandomWeaponHealthPlus20(); break;
            case UpgradeType.WeaponDamagePlus15: ApplyWeaponDamagePlus15(); break;
            case UpgradeType.TetrisHealthPlus25: ApplyTetrisHealthPlus25(); break;
            case UpgradeType.ZombieDamageMinus10: ApplyZombieDamageMinus10(); break;
            case UpgradeType.DropRandomTetris: ApplyDropRandomTetris(); break;
            case UpgradeType.CoinPlus100: ApplyCoinPlus100(); break;
            case UpgradeType.ZombiesSlower5Percent: ApplyZombiesSlower5Percent(); break;
            case UpgradeType.CoinGainPlus20Percent: ApplyCoinGainPlus20Percent(); break;
            case UpgradeType.WallHealthPlus30: ApplyWallHealthPlus30(); break;
            case UpgradeType.TetrisBlockHealthPlus1: ApplyTetrisBlockHealthPlus1(); break;
            case UpgradeType.RandomBlockRepair: ApplyRandomBlockRepair(); break;
            case UpgradeType.CritChancePlus10: ApplyCritChancePlus10(); break;
            case UpgradeType.BallPierceTwoEnemies: ApplyBallPierceTwoEnemies(); break;
            case UpgradeType.CoinPlus2PerKill: ApplyCoinPlus2PerKill(); break;
            case UpgradeType.ExtraWeaponSlot: ApplyExtraWeaponSlot(); break;
            case UpgradeType.ZombiesFaster10CoinX2: ApplyZombiesFaster10CoinX2(); break;
        }
    }

    private void ApplyHealthPlus20() {
        float UpgradeValue = Fence.HP * 20 / 100;

        if (Fence.HPMax > UpgradeValue)
        {
            Fence.HP += UpgradeValue;
        }
        else
        {
            Fence.HP = Fence.HPMax;
            Debug.Log("1 func");
        }
    }
    private void ApplyBallSpeedPlus20()
    {
        Debug.Log("2 func");
    }
    private void ApplyAllWeaponsHealthPlus10()
    {
        Debug.Log("3 func");
    }
    private void ApplyCoinPlus50()
    {
        UIManager.Instance.SetCoins(50);
        Debug.Log("4 func");
    }
    private void ApplyBallAccelerationPlus10()
    {
        Debug.Log("5 func");
    }
    private void ApplyExtraBall()
    {
        Debug.Log("6 func");
    }
    private void ApplyRandomWeaponHealthPlus20()
    {
        Debug.Log("7 func");
    }
    private void ApplyWeaponDamagePlus15()
    {
        Debug.Log("8 func");
    }
    private void ApplyTetrisHealthPlus25()
    {
        Debug.Log("9 func");
    }
    private void ApplyZombieDamageMinus10()
    {
        Debug.Log("10 func");
    }
    private void ApplyDropRandomTetris()
    {
        Debug.Log("11 func");
    }
    private void ApplyCoinPlus100()
    {
        Debug.Log("12 func");
    }
    private void ApplyZombiesSlower5Percent()
    {
        Debug.Log("13 func");
    }
    private void ApplyCoinGainPlus20Percent()
    {
        Debug.Log("14 func");
    }
    private void ApplyWallHealthPlus30()
    {
        Debug.Log("15 func");
    }
    private void ApplyTetrisBlockHealthPlus1()
    {
        Debug.Log("16 func");
    }
    private void ApplyRandomBlockRepair()
    {
        Debug.Log("17 func");
    }
    private void ApplyCritChancePlus10()
    {
        Debug.Log("18 func");
    }
    private void ApplyBallPierceTwoEnemies()
    {
        Debug.Log("19 func");
    }
    private void ApplyCoinPlus2PerKill()
    {
        Debug.Log("20 func");
    }
    private void ApplyExtraWeaponSlot()
    {
        Debug.Log("21 func");
    }
    private void ApplyZombiesFaster10CoinX2()
    {
        Debug.Log("22 func");
    }
}

[System.Serializable]
public class UpgradeData
{
    public UpgradeType Type;
    public string Description;
    public int CrystalCost;
}

public enum UpgradeType
{
    HealthPlus20,
    BallSpeedPlus20,
    AllWeaponsHealthPlus10,
    CoinPlus50,
    BallAccelerationPlus10,
    ExtraBall,
    RandomWeaponHealthPlus20,
    WeaponDamagePlus15,
    TetrisHealthPlus25,
    ZombieDamageMinus10,
    DropRandomTetris,
    CoinPlus100,
    ZombiesSlower5Percent,
    CoinGainPlus20Percent,
    WallHealthPlus30,
    TetrisBlockHealthPlus1,
    RandomBlockRepair,
    CritChancePlus10,
    BallPierceTwoEnemies,
    CoinPlus2PerKill,
    ExtraWeaponSlot,
    ZombiesFaster10CoinX2
}
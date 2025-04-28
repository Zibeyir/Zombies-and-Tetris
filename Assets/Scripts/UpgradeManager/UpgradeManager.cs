using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Prefab and Parents")]
    public GameObject UpgradeUIPrefab;
    public GameObject BallPrefab;
    public Transform BallTransformPrefab;

    public RectTransform[] UpgradeUIParents;

    public Dictionary<UpgradeType, UpgradeData> upgradeLookup;

    [Header("Upgrade UI Settings")]
    [SerializeField] private GameObject UpgradePanel;
    [SerializeField] private List<Sprite> UpgradeSprites; // <<<<< SPRITES LISTƏSİ BURADA

    private void Awake()
    {
        UpgradePanel.SetActive(false);

        InitializeUpgradeData();
        AssignSpritesToUpgrades(); // <<<<< SPRITE'LARI OTO OTURDURUQ
    }

    public void OnEnableUpgradeScene()
    {
        UpgradePanel.SetActive(true);
        GenerateRandomUpgrades();
    }

    private void InitializeUpgradeData()
    {
        upgradeLookup = new Dictionary<UpgradeType, UpgradeData>
        {
            { UpgradeType.HealthPlus20, new UpgradeData { Type = UpgradeType.HealthPlus20, Description = "+20% Health", CrystalCost = 55 } },
            { UpgradeType.AllWeaponsHealthPlus10, new UpgradeData { Type = UpgradeType.AllWeaponsHealthPlus10, Description = "+10 All Weapons Health", CrystalCost = 125 } },
            { UpgradeType.CoinPlus50, new UpgradeData { Type = UpgradeType.CoinPlus50, Description = "+50 Coins", CrystalCost = 20 } },
            { UpgradeType.ExtraBall, new UpgradeData { Type = UpgradeType.ExtraBall, Description = "+1 Extra Ball", CrystalCost = 80 } },
            { UpgradeType.WeaponDamagePlus15, new UpgradeData { Type = UpgradeType.WeaponDamagePlus15, Description = "+15 Weapon Damage", CrystalCost = 75 } },
            { UpgradeType.TetrisHealthPlus25, new UpgradeData { Type = UpgradeType.TetrisHealthPlus25, Description = "+25 Tetris Block Health", CrystalCost = 165 } },
            { UpgradeType.CoinPlus100, new UpgradeData { Type = UpgradeType.CoinPlus100, Description = "+100 Coins", CrystalCost = 35 } },
            { UpgradeType.CoinGainPlus20Percent, new UpgradeData { Type = UpgradeType.CoinGainPlus20Percent, Description = "+20% Coin Gain", CrystalCost = 45 } },
            { UpgradeType.WallHealthPlus30, new UpgradeData { Type = UpgradeType.WallHealthPlus30, Description = "+30 Wall Health", CrystalCost = 50 } },
            { UpgradeType.CoinPlus2PerKill, new UpgradeData { Type = UpgradeType.CoinPlus2PerKill, Description = "+2 Coins per Kill", CrystalCost = 60 } },
        };
    }

    private void AssignSpritesToUpgrades()
    {
        int index = 0;
        foreach (var pair in upgradeLookup)
        {
            if (index < UpgradeSprites.Count)
            {
                pair.Value.Icon = UpgradeSprites[index];
                index++;
            }
            else
            {
                Debug.LogWarning("Not enough sprites assigned for all upgrades!");
                break;
            }
        }
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

            GameObject obj = Instantiate(UpgradeUIPrefab, parent);

            UpgradeUIPrefabController controller = obj.GetComponent<UpgradeUIPrefabController>();
            controller.gameObject = UpgradePanel;
            if(count == 2)
            {
                controller.BuyPart.SetActive(true);
            }
            else
            {
                controller.BuyPart.SetActive(false);
            }
            if (controller != null)
            {
                controller.SetUpgradeData(data.Description, data.CrystalCost, () => OnUpgradeSelected(data.Type), data.Icon); // <<< iconu da ötürmək lazımdır
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
            case UpgradeType.AllWeaponsHealthPlus10: ApplyAllWeaponsHealthPlus10(); break;
            case UpgradeType.CoinPlus50: ApplyCoinPlus50(); break;
            case UpgradeType.ExtraBall: ApplyExtraBall(); break;
            case UpgradeType.TetrisHealthPlus25: ApplyTetrisHealthPlus25(); break;
            case UpgradeType.CoinPlus100: ApplyCoinPlus100(); break;
            case UpgradeType.CoinGainPlus20Percent: ApplyCoinGainPlus20Percent(); break;
            case UpgradeType.WallHealthPlus30: ApplyWallHealthPlus30(); break;
            case UpgradeType.CoinPlus2PerKill: ApplyCoinPlus2PerKill(); break;
        }
    }

    private void ApplyHealthPlus20()
    {
        float UpgradeValue = Fence.HP * 20 / 100;

        if (Fence.HPMax > UpgradeValue)
        {
            Fence.HP += UpgradeValue;
        }
        else
        {
            Fence.HP = Fence.HPMax;
        }
    }

    private void ApplyAllWeaponsHealthPlus10()
    {
        Debug.Log("3 func");
    }

    private void ApplyCoinPlus50()
    {
        UIManager.Instance.SetCoins(50);
    }

    private void ApplyExtraBall()
    {
        Instantiate(BallPrefab, BallTransformPrefab.position, Quaternion.identity);
    }

    private void ApplyTetrisHealthPlus25()
    {
        Debug.Log("9 func");
    }

    private void ApplyCoinPlus100()
    {
        UIManager.Instance.SetCoins(100);
    }

    private void ApplyCoinGainPlus20Percent()
    {
        CoinUI.counPercent += 20;
    }

    private void ApplyWallHealthPlus30()
    {
        //Fence.TakeDamage(-30);
    }

    private void ApplyCoinPlus2PerKill()
    {
        Debug.Log("20 func");
    }
}

[System.Serializable]
public class UpgradeData
{
    public UpgradeType Type;
    public string Description;
    public int CrystalCost;
    public Sprite Icon; // <<< ICONU BURADA ƏLAVƏ EDEDİK
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

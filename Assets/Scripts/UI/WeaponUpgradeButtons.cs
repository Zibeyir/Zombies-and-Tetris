using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WeaponUpgradeButtons : MonoBehaviour
{
    public WeaponUpgrade weaponUpgrade;
    WeaponData weaponData;
    public WeaponType Type;                  // Enum identifikator
    int Attack;
    bool UnlockCondition;
    int UnlockCost;
    int Level;// [base, level2, ..., level5]
    int UpgradeCosts;

    [SerializeField] private TextMeshProUGUI AttackText;
    [SerializeField] private TextMeshProUGUI UnlockConditionText;
    [SerializeField] private TextMeshProUGUI UnlockAndUpgradeCostText;
    [SerializeField] private TextMeshProUGUI LevelText;
    [SerializeField] private Button upgradeButton;

    public void GetWeaponUpgrateDetailes(WeaponData _weaponData)
    {
        weaponData = _weaponData;
        UnlockCondition = _weaponData.UnlockCondition;
        UnlockCost = _weaponData.UnlockCost;
        Level = _weaponData.Level;
        if(Level > 4) Level = 4; // max level is 5
        Attack = _weaponData.Damages[Level-1];
        UpgradeCosts = _weaponData.UpgradeCosts[Level-1];

        AttackText.text = Attack.ToString();
        UnlockConditionText.text = UnlockCondition.ToString();
        UnlockAndUpgradeCostText.text = UnlockCost.ToString();
        LevelText.text = Level.ToString();

        CheckConditions();
        //Debug.Log("Weapon Type: " + Type);
        //Debug.Log("Attack: " + Attack);
        //Debug.Log("Unlock Condition: " + UnlockCondition);
        //Debug.Log("Unlock Cost: " + UnlockCost);
        //Debug.Log("Level: " + Level);
        //Debug.Log("Upgrade Costs: " + UpgradeCosts);
    }

    void CheckConditions()
    {
        // Check if the weapon is unlocked
        if (!UnlockCondition)
        {
            UnlockConditionText.text = "UNLOCK";
            UnlockAndUpgradeCostText.text = UnlockCost.ToString();
            AttackText.text = " ";
            LevelText.text = " ";
            // Check if the player has enough resources to unlock/upgrade
            if (LevelManager.Instance._SaveData.Cristal >= UnlockCost)
            {
                // Enable the button
                upgradeButton.interactable = true;
            }
            else
            {
                // Disable the button
                upgradeButton.interactable = false;
            }
        }
        else
        {
            AttackText.text ="+"+ Attack.ToString()+" attack";
            UnlockConditionText.text = " ";
            UnlockAndUpgradeCostText.text = UpgradeCosts.ToString();
            LevelText.text = "LVL "+Level.ToString();
            if (LevelManager.Instance._SaveData.Cristal >= UpgradeCosts)
            {
                // Enable the button
                upgradeButton.interactable = true;
            }
            else
            {
                // Disable the button
                upgradeButton.interactable = false;
            }
           
            // Disable the button
            // upgradeButton.interactable = false;
        }
    }
    public void OnClickUpgradeButton()
    {
        // if()
        if (UnlockCondition)
        {
            weaponData.Level++;
           Debug.Log(weaponData.Level+" Level Weapon");
        }
        else
        {

            weaponData.UnlockCondition = true;
            Debug.Log(weaponData.UnlockCondition + " UnlockCondition");
        }
        weaponUpgrade.UpgradeorUnlockClick(weaponData);


        //Debug.Log("Upgrade");
        //GameEvents.OnUpgradeButtonClicked?.Invoke();
        //UIManager.Instance.OnEnableUpgradeScene();
    }
}

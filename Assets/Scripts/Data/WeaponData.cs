[System.Serializable]
public class WeaponData
{
    public string Type;                  // Enum identifikator
    public string Name;                      // Göstərilən ad (localized və ya display name)
    public int[] Damages;
    public bool UnlockCondition;
    public int UnlockCost;
    public int Level;// [base, level2, ..., level5]
    public int[] UpgradeCosts;

}

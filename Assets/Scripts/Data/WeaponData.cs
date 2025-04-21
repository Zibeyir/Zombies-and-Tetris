[System.Serializable]
public class WeaponData
{
    public string Type;                  // Enum identifikator
    public string Name;                      // Göstərilən ad (localized və ya display name)
    public int[] Damages;                    // [base, level2, ..., level5]
    public string UnlockCondition;
    public int UnlockCost;
}

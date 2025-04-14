using UnityEngine;
using System.IO;
using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int Coins;
    public List<int> UnlockedWeapons;
    public int CurrentLevel;
    public int PlayerHP;
    public List<int> WeaponLevels;
    // Add more fields as needed
}

public static class SaveDataService
{
    private static string SaveFilePath => Path.Combine(Application.persistentDataPath, "SaveData.json");

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SaveFilePath, json);
    }

    public static SaveData Load()
    {
        if (File.Exists(SaveFilePath))
        {
            string json = File.ReadAllText(SaveFilePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        return new SaveData(); // default
    }

    public static void DeleteSave()
    {
        if (File.Exists(SaveFilePath))
        {
            File.Delete(SaveFilePath);
        }
    }
}

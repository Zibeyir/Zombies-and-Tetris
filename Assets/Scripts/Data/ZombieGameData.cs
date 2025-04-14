using System.Collections.Generic;

[System.Serializable]
public class ZombieGameData
{
    public List<WeaponData> weapons;
    public List<EnemyData> enemies;
    public List<WaveData> waves;
    public List<GameStageData> gameStages;
    public List<UpgradeCostData> upgradeCosts;
    public List<MapData> maps;
}

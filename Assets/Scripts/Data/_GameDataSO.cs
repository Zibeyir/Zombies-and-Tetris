using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "Game/Game Data")]
public class _GameDataSO : ScriptableObject
{
    public int PlayerHealth;
    public int Coin;
    public int Cristal;

    public List<Transform> CurrentBlocks = new List<Transform>();
    public List<Transform> ActiveButtonBlocks = new List<Transform>();

    public List<Weapon> CurrentBlocksWeapons = new List<Weapon>();

    public WeaponType SelectedWeaponType;
}

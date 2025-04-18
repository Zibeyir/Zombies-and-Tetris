using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameTimeData : MonoBehaviour
{
    public static _GameTimeData Instance { get; private set; }

    public _GameDataSO Data;


    public int PlayerHealth;
    public int Coin;
    public int Cristal;

    public List<Transform> CurrentBlocks = new List<Transform>();
    public List<Transform> ActiveButtonBlocks = new List<Transform>();

    public List<Weapon> CurrentBlocksWeapons = new List<Weapon>();

    public WeaponType SelectedWeaponType;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        CurrentBlocksWeapons.Clear();
        CurrentBlocks.Clear();

    }
}

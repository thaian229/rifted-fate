using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    // Singleton
    public static GameManager instance;
    public int Creadit = 0;
    public int HealthLevel = 1;
    public int SpeedLevel = 1;
    public List<int> OwnedGuns;
    public int[] EquipedGuns = new int[2];
    public Gun[] AllGuns;
    public Dictionary<int, int> GunCost = new Dictionary<int, int>();
    public const int MaxUpgradeLevel = 10;
    public const int UpgradeCostByLevel = 500;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        // Other init
        this.OwnedGuns = new List<int>();
        this.OwnedGuns.Add(0);
        this.OwnedGuns.Add(1);
        this.EquipedGuns[0] = 0;
        this.EquipedGuns[1] = 1;

        GunCost.Add(0, 0);
        GunCost.Add(1, 0);
        GunCost.Add(2, 3000);
        GunCost.Add(3, 7000);
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadGameData();
        SceneController.instance.SwitchScenes("MainMenu");
    }

    public void AddCredit(int amount)
    {
        if (amount < 0) return;
        this.Creadit += amount;
        SaveGameData();
    }

    private bool SpendCredit(int amount)
    {
        if (amount > this.Creadit) return false;
        this.Creadit -= amount;
        return true;
    }

    // Shop feature
    public int GetUpgradeCostByLevel(int level)
    {
        if (level < 1) level = 1;
        else if (level >= MaxUpgradeLevel) return int.MaxValue;
        return level * UpgradeCostByLevel;
    }

    public void UpgradeHealth()
    {
        int cost = GetUpgradeCostByLevel(HealthLevel);
        if (!SpendCredit(cost)) return;
        if (this.HealthLevel >= MaxUpgradeLevel) return;
        this.HealthLevel += 1;
        SaveGameData();
    }

    public void UpgradeSpeed()
    {
        int cost = GetUpgradeCostByLevel(SpeedLevel);
        if (!SpendCredit(cost)) return;
        if (this.SpeedLevel >= MaxUpgradeLevel) return;
        this.SpeedLevel += 1;
        SaveGameData();
    }

    public void UnlockGun(int gunId)
    {
        // Check owned
        if (IsOwnedGun(gunId)) return;
        if (!SpendCredit(GunCost[gunId])) return;

        // Add gun
        OwnedGuns.Add(gunId);
        SaveGameData();
    }

    public void EquipGun(int gunId)
    {
        // Check owned
        if (!IsOwnedGun(gunId)) return;

        EquipedGuns[0] = gunId;
        SaveGameData();
    }

    public bool IsOwnedGun(int gunId)
    {
        for (int i = 0; i < OwnedGuns.Count; i++)
        {
            if (OwnedGuns[i] == gunId) return true;
        }
        return false;
    }

    public void OnQuitGame()
    {
        SaveGameData();
        Application.Quit();
    }

    public void SaveGameData()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGameData()
    {
        GameData gameData = SaveSystem.LoadGame();
        if (gameData != null)
        {
            this.Creadit = gameData.Credit;
            this.HealthLevel = gameData.HealthLevel;
            this.SpeedLevel = gameData.SpeedLevel;

            this.OwnedGuns = new List<int>();
            for (int i = 0; i < gameData.OwnedGuns.Length; i++)
            {
                this.OwnedGuns.Add(gameData.OwnedGuns[i]);
            }

            this.EquipedGuns = gameData.EquipedGuns;
        }
    }
}


using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

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
    public bool IsLogined = false;
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

    // GG Play
    public void LoginGGPlay()
    {
        PlayGamesPlatform.Instance.ManuallyAuthenticate(ProcessAuthentication);
    }

    internal void ProcessAuthentication(SignInStatus status)
    {
        if (status == SignInStatus.Success)
        {
            IsLogined = true;
        }
        else
        {
            IsLogined = false;
        }
    }

    public void CloudSave()
    {
        if (!IsLogined) return;
        OpenSavedGameSave("savedata");
    }

    void OpenSavedGameSave(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedSave);
    }

    public void OnSavedGameOpenedSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            byte[] savedData = SaveSystem.SerializeGameData(this);
            SaveGame(game, savedData, TimeSpan.MinValue);
        }
        else
        {
            // handle error
        }
    }

    void SaveGame(ISavedGameMetadata game, byte[] savedData, TimeSpan totalPlaytime)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;

        SavedGameMetadataUpdate.Builder builder = new SavedGameMetadataUpdate.Builder();
        builder = builder
            .WithUpdatedPlayedTime(totalPlaytime)
            .WithUpdatedDescription("Saved game at " + DateTime.Now);

        SavedGameMetadataUpdate updatedMetadata = builder.Build();
        savedGameClient.CommitUpdate(game, updatedMetadata, savedData, OnSavedGameWritten);
    }

    public void OnSavedGameWritten(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
        }
        else
        {
            // handle error
        }
    }

    public void CloudLoad()
    {
        if (!IsLogined) return;
        OpenSavedGameLoad("savedata");
    }

    void OpenSavedGameLoad(string filename)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.OpenWithAutomaticConflictResolution(filename, DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime, OnSavedGameOpenedLoad);
    }

    public void OnSavedGameOpenedLoad(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle reading or writing of saved game.
            LoadGameData(game);
        }
        else
        {
            // handle error
        }
    }

    void LoadGameData(ISavedGameMetadata game)
    {
        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ReadBinaryData(game, OnSavedGameDataRead);
    }

    public void OnSavedGameDataRead(SavedGameRequestStatus status, byte[] data)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            // handle processing the byte array data
            GameData gameData = SaveSystem.DeserializeGameData(data);
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
        else
        {
            // handle error
        }
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


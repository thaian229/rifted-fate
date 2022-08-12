using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public static MenuHandler instance;
    public GameObject MainPanel;
    public GameObject OfflinePanel;
    public GameObject OnlinePanel;
    public GameObject ShopPanel;
    public GameObject SettingPanel;
    public Text CreditDisplay;

    // For the shop
    public Button HealthUpgrade;
    public Text HealthLevel;
    public Text HealthUpgradeCost;
    public Button SpeedUpgrade;
    public Text SpeedLevel;
    public Text SpeedUpgradeCost;
    public Button Gun2Unlock;
    public Button Gun2Equip;
    public Text Gun2Cost;
    public Button Gun3Unlock;
    public Button Gun3Equip;
    public Text Gun3Cost;

    // For GG Play
    public Button LoginButton;
    public Button SaveButton;
    public Button LoadButton;

    private GameObject m_currentPanel;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }

    void Start()
    {
        if (OfflinePanel)
        {
            OfflinePanel.SetActive(false);
            OnlinePanel.SetActive(false);
            ShopPanel.SetActive(false);
            SettingPanel.SetActive(false);
            MainPanel.SetActive(true);
            m_currentPanel = MainPanel;
            CreditDisplay.text = "Credit: " + GameManager.instance.Creadit;
        }
    }

    public void OpenMainPanel()
    {
        m_currentPanel.SetActive(false);
        MainPanel.SetActive(true);
        m_currentPanel = MainPanel;
    }

    public void OpenShop()
    {
        m_currentPanel.SetActive(false);
        ShopPanel.SetActive(true);
        UpdateUIShop();
        m_currentPanel = ShopPanel;
    }

    public void OpenSetting()
    {
        m_currentPanel.SetActive(false);
        SettingPanel.SetActive(true);
        m_currentPanel = SettingPanel;
    }

    public void OpenOffline()
    {
        m_currentPanel.SetActive(false);
        OfflinePanel.SetActive(true);
        m_currentPanel = OfflinePanel;
    }

    public void OpenOnline()
    {
        m_currentPanel.SetActive(false);
        OnlinePanel.SetActive(true);
        m_currentPanel = OnlinePanel;
    }

    public void StartPlayLevel(string level)
    {
        SceneController.instance.SwitchScenes(level);
    }

    public void StartOnlineCreate()
    {
        SceneController.instance.SwitchScenes("EndlessSolo");
    }

    public void StartOnlineJoin()
    {
        SceneController.instance.SwitchScenes("EndlessSolo");
    }

    public void StartSoloEndless()
    {
        SceneController.instance.SwitchScenes("EndlessSolo");
    }

    public void QuitGameApp()
    {
        GameManager.instance.OnQuitGame();
    }

    public void BackToMainMenu()
    {
        SceneController.instance.SwitchScenes("MainMenu");
    }

    // Shop Handler
    public void UpdateUIShop()
    {
        GameManager gm = GameManager.instance;
        // Credit
        CreditDisplay.text = "Credit: " + gm.Creadit;

        // Stat Node
        HealthLevel.text = "Health LV " + gm.HealthLevel;
        int healthCost = gm.GetUpgradeCostByLevel(gm.HealthLevel);
        HealthUpgradeCost.text = "" + healthCost + " C";
        HealthUpgrade.interactable = gm.Creadit >= healthCost;

        SpeedLevel.text = "Speed LV " + gm.SpeedLevel;
        int speedCost = gm.GetUpgradeCostByLevel(gm.SpeedLevel);
        SpeedUpgradeCost.text = "" + speedCost + " C";
        SpeedUpgrade.interactable = gm.Creadit >= speedCost;

        // Gun Node
        int gun2Cost = gm.GunCost[2];
        if (gm.IsOwnedGun(2))
        {
            Gun2Cost.text = "Unlocked";
        }
        else
        {
            Gun2Cost.text = "" + gun2Cost + " C";
        }
        Gun2Unlock.interactable = !gm.IsOwnedGun(2) && gm.Creadit >= gun2Cost;
        Gun2Equip.interactable = gm.IsOwnedGun(2);

        int gun3Cost = gm.GunCost[3];
        if (gm.IsOwnedGun(3))
        {
            Gun3Cost.text = "Unlocked";
        }
        else
        {
            Gun3Cost.text = "" + gun3Cost + " C";
        }
        Gun3Unlock.interactable = !gm.IsOwnedGun(3) && gm.Creadit >= gun3Cost;
        Gun3Equip.interactable = gm.IsOwnedGun(3);
    }

    public void UpgradeHealthHandler()
    {
        GameManager.instance.UpgradeHealth();
        UpdateUIShop();
    }

    public void UpgradeSpeedHandler()
    {
        GameManager.instance.UpgradeSpeed();
        UpdateUIShop();
    }

    public void UnlockGunHandler(int gunId)
    {
        GameManager.instance.UnlockGun(gunId);
        UpdateUIShop();
    }

    public void EquipGunHandler(int gunId)
    {
        GameManager.instance.EquipGun(gunId);
        UpdateUIShop();
    }

    // GG Play
    public void Login()
    {
        GameManager.instance.LoginGGPlay();
        UpdateSettingUI();
    }

    public void Save()
    {
        GameManager.instance.CloudSave();
        UpdateSettingUI();
    }

    public void Load()
    {
        GameManager.instance.CloudLoad();
        UpdateSettingUI();        
    }

    private void UpdateSettingUI()
    {
        bool logined = GameManager.instance.IsLogined;
        LoginButton.interactable = !logined;
        SaveButton.interactable = logined;
        LoadButton.interactable = logined;
    }

    public void CheatCredit()
    {
        AdsHandler.instance.UserChoseToWatchAd();
        // GameManager.instance.AddCredit(1000);
        UpdateUIShop();
    }
}

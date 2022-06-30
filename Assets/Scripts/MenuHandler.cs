using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    public GameObject MainPanel;
    public GameObject OfflinePanel;
    public GameObject OnlinePanel;
    public GameObject ShopPanel;
    public GameObject SettingPanel;

    private GameObject m_currentPanel;

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

    public void StartOnline()
    {
        SceneController.instance.SwitchScenes("EndlessSolo");
    }

    public void StartSoloEndless()
    {
        SceneController.instance.SwitchScenes("EndlessSolo");
    }

    public void QuitGameApp()
    {
        Application.Quit();
    }

    public void BackToMainMenu()
    {
        SceneController.instance.SwitchScenes("MainMenu");
    }
}

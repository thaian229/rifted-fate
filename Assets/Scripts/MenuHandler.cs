using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.FPS.Game;

public class MenuHandler : MonoBehaviour
{
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

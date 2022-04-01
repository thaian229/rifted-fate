using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Unity.FPS.Game
{
    public class SceneController : MonoBehaviour
    {
        public GameObject loadingScene;
        // Singleton
        public static SceneController instance;

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

            if (loadingScene != null)
            {
                DontDestroyOnLoad(loadingScene);
                loadingScene.SetActive(false);
            }
        }

        public void SwitchScenes(string sceneName)
        {
            if (loadingScene != null)
            {
                loadingScene.SetActive(true);
            }
            SceneManager.LoadScene(sceneName);
            if (loadingScene != null)
            {
                loadingScene.SetActive(false);
            }
        }
    }
}

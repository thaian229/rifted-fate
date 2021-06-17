using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
	public GameObject loadingScene;
    // Singleton
	public static SceneController instance;
	
    // Use this for initialization
	void Awake () 
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
	
	public IEnumerator SwitchScenes(string sceneName)
	{
		if (loadingScene != null)
		{
			loadingScene.SetActive(true);
		}
		int oldScene = SceneManager.GetActiveScene().buildIndex;
		yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
		yield return SceneManager.UnloadSceneAsync(oldScene);
	}

	public IEnumerator LoadSceneAndSetActive(string sceneName)
	{
		yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
		Scene newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount  - 1);
		SceneManager.SetActiveScene(newlyLoadedScene);
		if (loadingScene != null)
		{
			loadingScene.SetActive(false);
		}
	}
}

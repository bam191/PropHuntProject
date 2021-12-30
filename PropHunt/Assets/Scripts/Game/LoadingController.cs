using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Unity.Netcode.NetworkSceneManager;

public class LoadingController : Singleton<LoadingController>
{
    private const string GAME_SCENE_NAME = "Game";
    private const string MAIN_MENU_SCENE_NAME = "MainMenu";

    private Scene? _loadedLevel;

    public Action OnSceneLoadedCallback;

    public override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnExternalSceneLoaded;
        
        base.Initialize();
    }

    private void Update()
    {
        if (!NetworkManager.Singleton.IsServer) return;
        
        if (Input.GetKeyDown("f"))
        {
            LoadServerLevel("TestLevel");
        }
        
        if (Input.GetKeyDown("r"))
        {
            LoadServerLevel("TestLevel2");
        }
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME, LoadSceneMode.Single);
    }

    public void LoadMainMenu()
    {
        LobbyController.Instance.Disconnect();
        
        SceneManager.LoadScene(MAIN_MENU_SCENE_NAME, LoadSceneMode.Single);
    }

    public void LoadServerLevel(string levelName)
    {
        StartCoroutine(LoadLevelCoroutine(levelName));
    }

    private IEnumerator LoadLevelCoroutine(string levelName)
    {
        SceneEventProgressStatus sceneProgressStatus = SceneEventProgressStatus.None;

        if (_loadedLevel != null)
        {
            while (sceneProgressStatus != SceneEventProgressStatus.Started)
            {
                sceneProgressStatus = NetworkManager.Singleton.SceneManager.UnloadScene((Scene)_loadedLevel);
                yield return null;
            }

            _loadedLevel = null;
        }

        sceneProgressStatus = SceneEventProgressStatus.None;

        while(sceneProgressStatus != SceneEventProgressStatus.Started)
        {
            sceneProgressStatus = NetworkManager.Singleton.SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
            yield return null;
        }

        _loadedLevel = SceneManager.GetSceneByName(levelName);
    }

    private void OnExternalSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnSceneLoadedCallback?.Invoke();
    }
}

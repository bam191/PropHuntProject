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

    public void LoadServerLevel(string levelName)
    {
        if (_loadedLevel != null)
        {
            UnloadCurrentServerLevel();
            InternalLoadServerLevel(levelName);
        }
        else
        {
            InternalLoadServerLevel(levelName);
        }
    }

    private void InternalLoadServerLevel(string levelName)
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete += OnServerSceneLoaded;
        NetworkManager.Singleton.SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        Debug.LogError("load scene " + levelName);
    }

    private void OnServerSceneLoaded(ulong clientId, string levelName, LoadSceneMode loadSceneMode)
    {
        Debug.LogError("on loaded " + levelName);
        NetworkManager.Singleton.SceneManager.OnLoadComplete -= OnServerSceneLoaded;
        _loadedLevel = SceneManager.GetSceneByName(levelName);
    }

    public void LoadLevel(string levelName)
    {
        if (_loadedLevel != null)
        {
            UnloadCurrentLevel((_) => 
            {
                InternalLoadLevel(levelName);
            });
        }
        else
        {
            InternalLoadLevel(levelName);
        }
    }

    private void OnExternalSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        OnSceneLoadedCallback?.Invoke();
    }

    private void InternalLoadLevel(string levelName)
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _loadedLevel = scene;
    }

    private void OnSceneUnloaded(Scene scene)
    {
        if (scene == _loadedLevel)
        {
            _loadedLevel = null;
        }
    }

    private void UnloadCurrentLevel(Action<AsyncOperation> onUnloaded)
    {
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.UnloadSceneAsync((Scene)_loadedLevel).completed += onUnloaded;
    }

    private void UnloadCurrentServerLevel()
    {
        Debug.LogError("server unloading " + _loadedLevel?.name);
        NetworkManager.Singleton.SceneManager.UnloadScene((Scene)_loadedLevel);
        _loadedLevel = null;
        //onUnloaded?.Invoke(null);
    }
}

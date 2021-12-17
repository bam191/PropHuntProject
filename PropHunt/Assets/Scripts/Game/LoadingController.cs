using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    private static LoadingController _instance;
    public static LoadingController Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<LoadingController>(true);
            }

            return _instance;
        }
    }

    private const string GAME_SCENE_NAME = "Game";

    private Scene? _loadedLevel;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(GAME_SCENE_NAME, LoadSceneMode.Single);
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
}

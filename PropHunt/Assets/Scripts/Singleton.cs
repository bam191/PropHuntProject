using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T _instance;

    public static T Instance => _instance;

    public static T GetInstance()
    {
        if (_instance == null)
        {
            _instance = FindObjectOfType<T>();
            if (_instance == null)
            {
                GameObject singleton = new GameObject();
                _instance = singleton.AddComponent<T>();
                singleton.name = "[singleton] " + typeof(T).ToString();
            }
        }
        return _instance;
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance.gameObject != gameObject)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = (T)(object)this;
        }
        Initialize();
    }
    
    /// <summary>
    /// Initialization override
    /// Use this if you want to have code that runs in Awake()
    /// </summary>
    public virtual void Initialize() { }

}
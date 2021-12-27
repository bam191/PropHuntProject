using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<T>();
                if (_instance == null)
                {
                    Debug.LogError($"No monobehavior exists in the scene for: {typeof(T)}");
                }
            }
            return _instance;
        }
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
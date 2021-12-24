using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour
{
    private static T _instance;
    public static T Instance
    {
        get
        {
            return _instance;
        }
    }
    
    public static void SetInstance(T instance)
    {
        if (_instance == null)
            _instance = instance;
    }
}

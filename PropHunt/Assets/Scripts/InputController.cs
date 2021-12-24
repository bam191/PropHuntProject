using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    private static InputController _instance;
    public static InputController Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
    }
}

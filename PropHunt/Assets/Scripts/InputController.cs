using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : Singleton<InputController>
{
    public override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        
    }
}


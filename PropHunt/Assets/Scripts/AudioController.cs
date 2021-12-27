using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Singleton<AudioController>
{
    public override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        base.Initialize();
    }
}

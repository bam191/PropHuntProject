using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtController : Singleton<LookAtController>
{
    private List<LookAtCamera> _looksAts = new List<LookAtCamera>();
    private GameObject _activeCamera;

    public override void Initialize()
    {
        DontDestroyOnLoad(gameObject);
        base.Initialize();
    }

    public void RegisterLookAt(LookAtCamera lookAt)
    {
        _looksAts.Add(lookAt);
        lookAt.SetActiveCamera(_activeCamera);
    }

    public void UnregisterLookAt(LookAtCamera lookAt)
    {
        _looksAts.Remove(lookAt);
    }

    public void SetActiveCamera(GameObject camera)
    {
        foreach(LookAtCamera lookAt in _looksAts)
        {
            lookAt.SetActiveCamera(camera);
        }

        _activeCamera = camera;
    }
}

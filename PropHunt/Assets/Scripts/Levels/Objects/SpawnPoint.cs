using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eTeam
{
    Any,
    Props,
    Hunters
}

public class SpawnPoint : LevelObject
{
    [Tooltip("Higher priority spawn points are selected first")]
    [SerializeField] private int _priority;
    [Tooltip("Which team can spawn using this spawn point")]
    [SerializeField] private eTeam _team;

    public int Priority => _priority;
    public eTeam Team => _team;

    private void Start()
    {
        SpawnController.Instance.RegisterSpawnPoint(this);
    }

    private void OnDestroy()
    {
        SpawnController.Instance.DeregisterSpawnPoint(this);
    }
}

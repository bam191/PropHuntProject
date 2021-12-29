using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : LevelObject
{
    public enum eTeam
    {
        Any,
        Props,
        Hunters
    }

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

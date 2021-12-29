using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpawnController : Singleton<SpawnController>
{
    private List<SpawnPoint> _spawnPoints;

    private List<SpawnPoint> _availablePoints;
    private List<SpawnPoint> _usedPoints;

    public override void Initialize()
    {
        _spawnPoints = new List<SpawnPoint>();
        base.Initialize();
    }

    public void RegisterSpawnPoint(SpawnPoint spawnPoint)
    {
        _spawnPoints.Add(spawnPoint);

        _availablePoints.Add(spawnPoint);
        _availablePoints.OrderBy(x => x.Priority);
    }

    public void DeregisterSpawnPoint(SpawnPoint spawnPoint)
    {
        if (_availablePoints.Contains(spawnPoint))
        {
            _availablePoints.Remove(spawnPoint);
        }

        if (_spawnPoints.Contains(spawnPoint))
        {
            _spawnPoints.Remove(spawnPoint);
        }

        if (_usedPoints.Contains(spawnPoint))
        {
            _usedPoints.Remove(spawnPoint);
        }
    }

    public void ResetSpawnPoints()
    {
        _availablePoints.Clear();
        _availablePoints.AddRange(_spawnPoints);
        _availablePoints.OrderBy(x => x.Priority);
    }

    public SpawnPoint GetSpawnPoint(eTeam team, bool tryReset = true)
    {
        foreach(SpawnPoint spawnPoint in _availablePoints)
        {
            if (spawnPoint.Team == team || spawnPoint.Team == eTeam.Any || team == eTeam.Any)
            {
                _availablePoints.Remove(spawnPoint);
                _usedPoints.Add(spawnPoint);
                return spawnPoint;
            }
        }

        if (tryReset)
        {
            ResetAvailablePoints(team);
            return GetSpawnPoint(team, false);
        }

        Debug.LogError("No valid spawn points for " + team);
        return null;
    }

    private void ResetAvailablePoints(eTeam team)
    {
        foreach (SpawnPoint spawnPoint in _usedPoints)
        {
            if (spawnPoint.Team == team || spawnPoint.Team == eTeam.Any || team == eTeam.Any)
            {
                _usedPoints.Remove(spawnPoint);
                _availablePoints.Add(spawnPoint);
            }
        }

        _availablePoints.OrderBy(x => x.Priority);
    }
}

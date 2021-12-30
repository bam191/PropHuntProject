using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using Random = UnityEngine.Random;

public enum eGameState
{
    Setup, WaitingForPlayers, PreRound, HideRound, SeekingRound, RoundEnd, GameEnd
}
public class GameController : Singleton<GameController>
{
    private NetworkManager _networkManager;

    private List<PlayerController> _players;

    [SerializeField] private eGameState _currentState;

    [SerializeField] private GameObject _lobbySettingsPrefab;

    public NetworkVariable<float> roundTimer = new NetworkVariable<float>(0f);
    public NetworkVariable<bool> areHuntersDead = new NetworkVariable<bool>(false);
    public NetworkVariable<bool> arePropsDead = new NetworkVariable<bool>(false);
    public NetworkVariable<int> roundCount = new NetworkVariable<int>(0);

    public override void Initialize()
    {
        _networkManager = NetworkManager.Singleton;
        base.Initialize();

        _players = new List<PlayerController>();

        if (NetworkManager.Singleton.IsServer)
        {
            SetState(eGameState.Setup);
        }
    }

    private void Update()
    {
        CheckStateMigration();
    }

    private void CheckStateMigration()
    {
        roundTimer.Value -= Time.deltaTime;

        switch (_currentState)
        {
            case eGameState.PreRound:
                if (roundTimer.Value <= 0)
                {
                    SetState(eGameState.HideRound);
                }
                break;
            case eGameState.HideRound:
                if (roundTimer.Value <= 0)
                {
                    SetState(eGameState.SeekingRound);
                }
                break;
            case eGameState.SeekingRound:
                if (roundTimer.Value <= 0 || areHuntersDead.Value || arePropsDead.Value)
                {
                    SetState(eGameState.RoundEnd);
                }
                break;
            case eGameState.RoundEnd:
                if (roundTimer.Value <= 0 )
                {
                    if (roundCount.Value == LobbyController.Instance.LobbyData.numberOfRounds)
                    {
                        
                    }
                }
                break;
        }
    }
    
    public void SetState(eGameState newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case eGameState.Setup:
                Setup();
                break;
            case eGameState.WaitingForPlayers:
                WaitingForPlayers();
                break;
            case eGameState.PreRound:
                PreRound();
                break;
            case eGameState.HideRound:
                HideRound();
                break;
            case eGameState.SeekingRound:
                SeekRound();
                break;
            case eGameState.RoundEnd:
                RoundEnd();
                break;
            case eGameState.GameEnd:
                GameEnd();
                break;
            
        }
    }

    /// <summary>
    /// Take the lobby information and set up the server's settings
    /// For example: map, move speed, game mode, etc
    /// </summary>
    private void Setup()
    {
        Instantiate(_lobbySettingsPrefab);
        SetState(eGameState.WaitingForPlayers);
    }

    /// <summary>
    /// Send all players to free cam, start a timer
    /// </summary>
    private void WaitingForPlayers()
    {
        // When host presses start, or time is up
        SetState(eGameState.PreRound);
    }

    #region PreRound
    /// <summary>
    /// Assign players to teams
    /// Move players to their starting points
    /// Spawn all props
    /// </summary>
    private void PreRound()
    {
        roundTimer.Value = LobbyController.Instance.LobbyData.preRoundLength;

        AssignTeams();
        LoadMap();
        SpawnPlayers();
    }

    private void AssignTeams()
    {
        int numberOfSeekers = _players.Count / LobbyController.Instance.LobbyData.propsPerSeeker;

        List<PlayerController> availablePlayers = new List<PlayerController>(_players);
        List<PlayerController> seekers = new List<PlayerController>();
        
        while (numberOfSeekers > 0)
        {
            int index = Random.Range(0, availablePlayers.Count);
            seekers.Add(_players[index]);
            availablePlayers.Remove(_players[index]);
            numberOfSeekers--;
        }

        foreach (PlayerController player in _players)
        {
            if (seekers.Contains(player))
            {
                player.SetTeam(eTeam.Hunters);
            }
            else
            {
                player.SetTeam(eTeam.Props);
            }
        }
    }

    private void LoadMap()
    {
        LoadingController.Instance.LoadServerLevel(LobbyController.Instance.LobbyData.mapName);
    }

    private void SpawnPlayers()
    {
        SpawnController.Instance.ResetSpawnPoints();
        foreach (PlayerController player in _players)
        {
            SpawnPoint spawnPoint = SpawnController.Instance.GetSpawnPoint(player.team.Value);
            player._requestedPosition.Value = spawnPoint.transform.position;
        }
    }
    #endregion
    
    /// <summary>
    /// Disable seeker vision/movement
    /// Enable prop vision/movement
    /// Start timer
    /// </summary>
    private void HideRound()
    {
        roundTimer.Value = LobbyController.Instance.LobbyData.hideRoundLength;

        InitPlayerState();
    }
    
    private void InitPlayerState()
    {
        foreach (PlayerController player in _players)
        {
            if (player.team.Value == eTeam.Hunters)
            {
                // Disable vision
            }else if (player.team.Value == eTeam.Props)
            {
                // Assign random prop
            }
        }
    }

    /// <summary>
    /// Enable seekers
    /// </summary>
    private void SeekRound()
    {
        roundTimer.Value = LobbyController.Instance.LobbyData.seekRoundLength;
        EnableHunterVision();
    }

    private void EnableHunterVision()
    {
        
    }

    /// <summary>
    /// Reveal remaining props
    /// Show scoreboard
    /// Check remaining rounds, end game if over
    /// </summary>
    private void RoundEnd()
    {
        // when rounds remaining (after a delay)
        //SetState(eGameState.PreRound);
        // when rounds are done
        SetState(eGameState.GameEnd);
    }

    /// <summary>
    /// Show scoreboard
    /// Map voting
    /// </summary>
    private void GameEnd()
    {
        // Closes the lobby, or goes to map voting
    }
    
    
    public void RegisterPlayer(PlayerController player)
    {
        _players.Add(player);
    }

}

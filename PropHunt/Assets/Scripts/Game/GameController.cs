using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

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

    public void RegisterPlayer(PlayerController player)
    {
        _players.Add(player);
    }

    public void SetState(eGameState newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case eGameState.Setup:
                StartCoroutine(Setup());
                break;
            case eGameState.WaitingForPlayers:
                StartCoroutine(WaitingForPlayers());
                break;
            case eGameState.PreRound:
                StartCoroutine(PreRound());
                break;
            case eGameState.HideRound:
                StartCoroutine(HideRound());
                break;
            case eGameState.SeekingRound:
                StartCoroutine(SeekRound());
                break;
            case eGameState.RoundEnd:
                StartCoroutine(RoundEnd());
                break;
            case eGameState.GameEnd:
                StartCoroutine(GameEnd());
                break;
            
        }
    }

    /// <summary>
    /// Take the lobby information and set up the server's settings
    /// For example: map, move speed, game mode, etc
    /// </summary>
    private IEnumerator Setup()
    {
        yield return new WaitForSeconds(1);
        Instantiate(_lobbySettingsPrefab);
        SetState(eGameState.WaitingForPlayers);
    }

    /// <summary>
    /// Send all players to free cam, start a timer
    /// </summary>
    private IEnumerator WaitingForPlayers()
    {
        // When host presses start, or time is up
        yield return new WaitForSeconds(1);
        
        SetState(eGameState.PreRound);
    }

    /// <summary>
    /// Assign players to teams
    /// Move players to their starting points
    /// Spawn all props
    /// </summary>
    private IEnumerator PreRound()
    {
        yield return new WaitForSeconds(1);
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
        
        SetState(eGameState.HideRound);
    }

    /// <summary>
    /// Disable seeker vision/movement
    /// Enable prop vision/movement
    /// Start timer
    /// </summary>
    private IEnumerator HideRound()
    {
        yield return new WaitForSeconds(1);
        // Move after time elapses
        SetState(eGameState.SeekingRound);
    }

    /// <summary>
    /// Enable seekers
    /// </summary>
    private IEnumerator SeekRound()
    {
        yield return new WaitForSeconds(1);
        // when:
        // - time runs out
        // - all seekers/props dead
        SetState(eGameState.RoundEnd);
    }

    /// <summary>
    /// Reveal remaining props
    /// Show scoreboard
    /// Check remaining rounds, end game if over
    /// </summary>
    private IEnumerator RoundEnd()
    {
        yield return new WaitForSeconds(1);
        // when rounds remaining (after a delay)
        //SetState(eGameState.PreRound);
        // when rounds are done
        SetState(eGameState.GameEnd);
    }

    /// <summary>
    /// Show scoreboard
    /// Map voting
    /// </summary>
    private IEnumerator GameEnd()
    {
        yield return new WaitForSeconds(1);
        // Closes the lobby, or goes to map voting
    }
}

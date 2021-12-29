using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LobbyMaps", menuName = "ScriptableObjects/LobbyMaps", order = 1)]
public class LobbyMapsSO : ScriptableObject
{
    public List<string> lobbyMaps;
}

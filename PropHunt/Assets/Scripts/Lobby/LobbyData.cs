using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[Serializable]
public struct LobbyData
{
    public int propsPerSeeker;
    public string mapName;
    
    public static byte[] GetBytes(LobbyData lobbyData)
    {
        int size = Marshal.SizeOf(lobbyData);
        byte[] bytes = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(lobbyData, ptr, true);
        Marshal.Copy(ptr, bytes, 0, size);
        Marshal.FreeHGlobal(ptr);

        return bytes;
    }

    public static LobbyData FromBytes(byte[] bytes)
    {
        LobbyData lobbyData = new LobbyData();

        int size = Marshal.SizeOf(lobbyData);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(bytes, 0, ptr, size);

        lobbyData = (LobbyData)Marshal.PtrToStructure(ptr, lobbyData.GetType());
        Marshal.FreeHGlobal(ptr);

        return lobbyData;
    }
    
    public void SetPropsPerSeeker(int propsPerSeeker)
    {
        this.propsPerSeeker = propsPerSeeker;
    }

    public void SetMap(string mapName)
    {
        this.mapName = mapName;
    }
}

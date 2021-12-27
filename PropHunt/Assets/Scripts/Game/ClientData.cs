using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

[Serializable]
public struct ClientData
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string Username;
    public ulong ClientId;

    public static byte[] GetBytes(ClientData clientData)
    {
        int size = Marshal.SizeOf(clientData);
        byte[] bytes = new byte[size];

        IntPtr ptr = Marshal.AllocHGlobal(size);
        Marshal.StructureToPtr(clientData, ptr, true);
        Marshal.Copy(ptr, bytes, 0, size);
        Marshal.FreeHGlobal(ptr);

        return bytes;
    }

    public static ClientData FromBytes(byte[] bytes)
    {
        ClientData clientData = new ClientData();

        int size = Marshal.SizeOf(bytes);
        IntPtr ptr = Marshal.AllocHGlobal(size);

        Marshal.Copy(bytes, 0, ptr, size);

        clientData = (ClientData)Marshal.PtrToStructure(ptr, clientData.GetType());
        Marshal.FreeHGlobal(ptr);

        return clientData;
    }
}

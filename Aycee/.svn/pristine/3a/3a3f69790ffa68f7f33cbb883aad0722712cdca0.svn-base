using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public static class UGlobalSettings
{
    public enum GameMode
    {
        SINGLEPLAYER = 1,
        MULTIPLAYER_JOIN,
        MULTIPLAYER_STARTSERVER
    }
    public static string serverIPAddress;
    public static GameMode gameMode=GameMode.SINGLEPLAYER;
    public static string pathUserData
    {
        get { return Application.dataPath+"/"; }
    }
    public static string pathGameConfig
    {
        get { return Application.streamingAssetsPath + "/Config/"; }
    }
    public static string pathGameData
    {
        get { return Application.streamingAssetsPath + "/Data/"; }
    }
}


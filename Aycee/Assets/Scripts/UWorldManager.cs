using System;
using UnityEngine;
using UnityEngine.Rendering;
using MLAPI;
using Assets.WorldEngine;

public class UWorldManager : NetworkBehaviour
{
    bool _gameStarted = false;
    string _pathGameConfig;
    static CWorld _world;
    int currentFPS, fpsUpdater;

    //PROPERTIES:
    public static CWorld world
    {
        get { return _world; }
    }

    void Awake()
    {
        _pathGameConfig = UGlobalSettings.pathGameConfig;
        Debug.Log("Loading configurations files from: "+ _pathGameConfig);

        //Initialise the whole world! :-)
        _world = new CWorld();
        _world.LoadWorldConfig(_pathGameConfig + "WorldConfig.xml");
        _world.LoadWorldData(UGlobalSettings.pathGameData);
        _world.InitWorld();
        Debug.Log("Init world. Active blocks: " + _world.activeBlocksCount);
        //_world.SaveWorldConfig("D:/Game Development/" + "WorldConfig.xml");
        //  _world.LoadChemicalEntities(UGlobalSettings.pathSaveGame + "ac_chemical_entities.xml");
        //_world.SaveChemicalEntities("D:/Game Development/" + "ac_chemical_entities.xml");
        //_world.LoadChemicalReactionRules(UGlobalSettings.pathGameData + "ac_chemical_reaction_rules.xml");
        //_world.SaveChemicalReactionRules(saveGamePath+"ac_chemical_reaction_rules.xml");
        currentFPS = 0;
        fpsUpdater = 0;
    }

    public void Start()
    {
    }

    
    void OnGUI()
    {
        if(!Input.GetKey(KeyCode.Tab))
        {
            return;
        }

        //Calculate FPS now:
        fpsUpdater++;
        if (fpsUpdater > 500)
        {
            currentFPS = (int)(1.0f / Time.unscaledDeltaTime);
            fpsUpdater = 0;
        }

        float menuX, menuY;
        float menuWidth, menuHeight;

        menuWidth = 200;
        menuHeight = 300;

        menuX = 10;
        menuY = 10;
        GUILayout.BeginArea(new Rect(menuX, menuY, menuWidth, menuHeight));
      //  if (!IsServer)
        {
            long memoryUsageKB = System.GC.GetTotalMemory(false) / 1024;
            long memoryUsageMB = memoryUsageKB / 1024;

            GUILayout.Label("Memory usage (MB): "+String.Format("{0:0,0}", memoryUsageMB));
            GUILayout.Label("FPS: " + currentFPS);
            GUILayout.Label("Active blocks: "+_world.activeBlocksCount);
            GUILayout.Label("Biomes: "+_world.biomeList.Count);
            GUILayout.Label("World width (meters): " + _world.horizontalExtent);
        }
        GUILayout.EndArea();
    }

    public void Update()
    {
        if(!_gameStarted)
        {
            switch (UGlobalSettings.gameMode)
            {
                case UGlobalSettings.GameMode.SINGLEPLAYER:
                    NetworkManager.Singleton.StartHost();
                    break;
                case UGlobalSettings.GameMode.MULTIPLAYER_JOIN:
                    NetworkManager.Singleton.StartClient();
                    break;
                case UGlobalSettings.GameMode.MULTIPLAYER_STARTSERVER:
                    NetworkManager.Singleton.StartServer();
                    break;
            }
            _gameStarted = true;
        }
    }
}

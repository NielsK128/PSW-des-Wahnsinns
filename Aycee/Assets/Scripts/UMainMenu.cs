using UnityEngine;
using UnityEngine.SceneManagement;
using MLAPI;

public class UMainMenu: NetworkBehaviour
{
    private void Awake()
    {
        Camera.main.backgroundColor = Color.black;
    }

    void OnGUI()
    {
        float menuX, menuY;
        float menuWidth, menuHeight;

        menuWidth = 200;
        menuHeight = 300;

        menuX = (Screen.width - menuWidth) / 2;
        menuY = (Screen.height - menuHeight) / 2;
        GUILayout.BeginArea(new Rect(menuX, menuY, menuWidth, menuHeight));
        if (!IsServer)
        {
            StartButtons();
        }
        GUILayout.EndArea();
    }
    static void StartButtons()
    {
        if (GUILayout.Button("Singleplayer")) Menu_Singleplayer();
        if (GUILayout.Button("Join Server")) Menu_Multiplayer_JoinServer();
        //if (GUILayout.Button("Start Headless Server")) Menu_Multiplayer_StartHeadlessServer();
        if (GUILayout.Button("Quit")) Menu_Quit();
    }

    //MENU-METHODS
    static void Menu_Singleplayer()
    {
        UGlobalSettings.gameMode = UGlobalSettings.GameMode.SINGLEPLAYER;
        SceneManager.LoadScene("GameScene");
    }
    static void Menu_Multiplayer_JoinServer()
    {
        UGlobalSettings.gameMode = UGlobalSettings.GameMode.MULTIPLAYER_JOIN;
        SceneManager.LoadScene("MultiplayerMenuScene");
    }
    static void Menu_Multiplayer_StartHeadlessServer()
    {
        UGlobalSettings.gameMode = UGlobalSettings.GameMode.MULTIPLAYER_STARTSERVER;
        SceneManager.LoadScene("GameScene");
    }
    static void Menu_Quit()
    {
        Application.Quit();
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;
//using MLAPI;

public class UMultiplayerMenu : MonoBehaviour
{
    string ipAddress;

    private void Awake()
    {
        Camera.main.backgroundColor = Color.black;
        ipAddress = "localhost";
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
        GUILayout.Label("JOIN SERVER");
        GUILayout.Label(" IP-Address");
        ipAddress = GUILayout.TextField(ipAddress);
        if (GUILayout.Button("Connect")) Menu_ConnectToServer();
        if (GUILayout.Button("Back")) Menu_GoBack();
        GUILayout.EndArea();
    }

    void Menu_ConnectToServer()
    {
        if (ipAddress.ToLower() == "localhost")
        {
            ipAddress = "127.0.0.1"; //localhost hook
        }
        UGlobalSettings.serverIPAddress = ipAddress;
        SceneManager.LoadScene("GameScene");
    }
    void Menu_GoBack()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}

using UnityEngine;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    bool inJoinRoom = false;
    bool visible = true;

    private int ListenPort
    {
        get { return 11245; }
    }

    private int localListen = -1;
    private List<ServerInfo> hosts = new List<ServerInfo>();

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            visible = !visible;
    }

    void OnGUI()
    {
        if (!visible) return;
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.BeginVertical();

        if (GUILayout.Button("Join Game"))
        {
            JoinGamePressed();
        }
        if (GUILayout.Button("Host Game"))
        {
            HostGamePressed();
        }
        if (GUILayout.Button("Talk to Browser"))
        {
            TalkButtonPressed();
        }
        
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();

        if (inJoinRoom)
        {
            GUILayout.BeginVertical();
            foreach (ServerInfo si in hosts)
            {
                if (GUILayout.Button(si.gameName + " : " + si.IpAddress))
                {
                    JoinGamePressed(si);
                }
            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

    private void TalkButtonPressed()
    {
        Application.ExternalEval("alert('Hey!');");
        Application.ExternalCall("onFinishedLoading");
    }

    private void JoinGamePressed(ServerInfo si)
    {
        Debug.Log("Connecting to " + si.IpAddress + ": " + si.port);
        Network.Connect(si.IpAddress, si.port);
    }

    private void JoinGamePressed()
    {
        hosts = StrifeMasterServer.GetMasterServerList();
    }

    private void HostGamePressed()
    {
        localListen = ListenPort;
        Network.InitializeServer(32, localListen, false);
        visible = false;
    }

    void OnServerInitialized()
    {
        StrifeMasterServer.RegisterWithMasterServer(localListen, "testGame", "testGame", "TestGameType");
    }
}

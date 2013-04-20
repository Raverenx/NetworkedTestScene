using UnityEngine;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour
{
    bool inJoinRoom = false;
    bool visible = true;

    private int ListenPort
    {
        get { return 25000; }
    }

    private int localListen = -1;
    private List<ServerInfo> hosts = new List<ServerInfo>();
    public GameObject playerPrefab;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            visible = !visible;
            inJoinRoom = false;
        }
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
                    ServerButtonPressed(si);
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

    private void ServerButtonPressed(ServerInfo si)
    {
        Debug.Log("Connecting to " + si.IpAddress + " : " + si.port);
        Network.Connect(si.IpAddress, si.port);
        inJoinRoom = false;
    }

    private void JoinGamePressed()
    {
        hosts = StrifeMasterServer.GetMasterServerList();
        inJoinRoom = true;
    }

    private void HostGamePressed()
    {
        localListen = ListenPort;
        Network.InitializeServer(32, localListen, false);
        visible = false;
        var server = new GameObject("Server").AddComponent<StrifeServer>();
        server.port = localListen;
        server.gameName = "testGame";
        server.gameDescription = "testGame";
        server.gametype = "testGameType";
    }

    void OnConnectedToServer()
    {
        Debug.Log("Connected successfully.");
        CreateLocalPlayerObject();
    }

    void OnServerInitialized()
    {
        if (SystemInfo.graphicsDeviceID != 0)
        {
            CreateLocalPlayerObject();
        }
    }

    private void CreateLocalPlayerObject()
    {
        var player = Network.Instantiate(playerPrefab, Vector3.zero, Quaternion.identity, int.Parse(Network.player.ToString()));
        var team = Globals.teamNumber;
        Color color = Color.gray;
        if (team == 2)
        {
            color = Color.red;
        }
        if (team == 1)
        {
            color = Color.blue;
        }
        ((GameObject)player).networkView.RPC("ChangeColor", RPCMode.AllBuffered, color.r, color.g, color.b);
    }
}

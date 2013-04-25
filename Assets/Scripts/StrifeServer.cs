using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class StrifeServer : MonoBehaviour
{
    public int port;
    public string gameName;
    public string gametype = "DefaultGameType";
    public string gameDescription;

    internal void StartHeadlessServer(int port, string name, string description)
    {
        Debug.Log("Starting headless server with name = " + name + " and description = " + description);
        Globals.isHeadlessServer = true;
        Network.InitializeServer(20, port, false);
        this.port = port;
        this.gameName = name;
        this.gameDescription = description;
        Globals.listenPort = port;
    }

    internal void StartRegularServer(int port, string name, string description)
    {
        Debug.Log("Starting regular server with name = " + name + " and description = " + description);
        Network.InitializeServer(20, port, false);
        this.port = port;
        this.gameName = name;
        this.gameDescription = description;
        Globals.listenPort = port;
    }

    void Start()
    {
        StartCoroutine(SendHeartbeat());
        StrifeMasterServer.RegisterWithMasterServer(port, gameName, gameDescription, gametype);
    }

    private IEnumerator SendHeartbeat()
    {
        while (true)
        {
            yield return new WaitForSeconds(5.0f);

            // TODO: Send the number of connected players, etc.
        }
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log(player.externalIP + " has disconnected. There are now " + Network.connections.Length + " players connected.");
        if (Network.connections.Length == 1 && Globals.isHeadlessServer)
        {
            TurnOffServer();
        }
    }

    void OnDisconnectedFromServer(NetworkDisconnection mode)
    {
        TurnOffServer();
        StrifeMasterServer.DeregisterWithMasterServer(this.port);
    }

    private void TurnOffServer()
    {
        Application.Quit();
    }
}
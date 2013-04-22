using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class UserIdSync : MonoBehaviour {

    public static int userId = -1;

    private Dictionary<NetworkPlayer, int> players = new Dictionary<NetworkPlayer, int>();

    public void SetUserId(string value)
    {
        int val;
        if (int.TryParse(value, out val))
        {
            userId = val;
            Debug.Log("Received userid of " + userId);
        }
        else
        {
            Debug.LogError("Invalid format for userid: <" + value + ">");
        }
    }

    public void SetUsername(string value)
    {
        PlayerPrefs.SetString("username", value);
        Debug.Log("Received username: " + value);
    }

    void OnConnectedToServer()
    {
        networkView.RPC("BroadcastUserId", RPCMode.Server, userId);
    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        StrifeMasterServer.NotifyPlayerLeft(players[player]);
        Network.RemoveRPCs(player);
        Network.RemoveRPCsInGroup(players[player]);
        Network.DestroyPlayerObjects(player);
    }

    [RPC]
    void BroadcastUserId(int id, NetworkMessageInfo info)
    {
        Debug.Log("Received user ID: " + info.sender.externalIP + " has id <" + id + ">");
        players[info.sender] = id;
        StrifeMasterServer.NotifyPlayerJoined(id);
        networkView.RPC("ReceivedId", info.sender);
    }

    [RPC]
    void ReceivedId()
    {
        if (PlayerPrefs.GetInt("teamNumber", -1) == -1)
        {
            networkView.RPC("GetTeam", RPCMode.Server);
        }
    }

    [RPC]
    void GetTeam(NetworkMessageInfo info)
    {
        Debug.Log("Trying to find a team for " + info.sender.externalIP);
        var allPlayers = Object.FindObjectsOfType(typeof(Player));
        Debug.Log("There are " + allPlayers.ToArray().Length + " players in total");
        foreach (Object p in allPlayers) { Debug.Log("Player team : " + ((Player)p).team); }
        var numTeamOne = allPlayers.Count(p => ((Player)p).team == 1);
        var numTeamTwo = allPlayers.Count(p => ((Player)p).team == 2);
        Debug.Log("There are  " + numTeamOne + " players on team one and " + numTeamTwo + " players on team two");
        if (numTeamOne < numTeamTwo)
        {
            this.networkView.RPC("SetTeam", info.sender, 1);
        }
        else
        {
            this.networkView.RPC("SetTeam", info.sender, 2);
        }
    }

    [RPC]
    void SetTeam(int team)
    {
        Debug.Log("My team is " + team + "! Now i can create a player.");
        PlayerPrefs.SetInt("teamNumber", team);
        MainMenu.Instance.CreateLocalPlayerObject();
    }
}

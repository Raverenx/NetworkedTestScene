using UnityEngine;
using System.Collections;

public class BrowserCommunicator : MonoBehaviour {

    IEnumerator Start()
    {
        Application.ExternalCall("finishedLoading");
        yield break;
    }

    void ConnectToServer(string message)
    {
        var split = message.Split('\n');
        var ipAddress = split[0];
        var port = int.Parse(split[1]);
        var teamNumber = 0;
        teamNumber = int.Parse(split[2]);

        ConnectToServer(ipAddress, port, teamNumber);
    }

    public static void ConnectToServer(string ipAddress, int port, int teamNumber)
    {
        PlayerPrefs.SetInt("teamNumber", teamNumber);
        Debug.Log("Connecting to " + ipAddress + " : " + port + " on team " + teamNumber);
        Network.Connect(ipAddress, port);
    }
}

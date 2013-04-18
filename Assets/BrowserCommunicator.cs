using UnityEngine;
using System.Collections;

public class BrowserCommunicator : MonoBehaviour {

    IEnumerator Start()
    {
        Application.ExternalEval("alert('Finished Loading!');");
        yield break;
    }

    void ConnectToGame(string message)
    {
        var split = message.Split('\n');
        var ipAddress = split[0];
        var port = int.Parse(split[1]);
        var teamNumber = int.Parse(split[2]);

        Network.Connect(ipAddress, port);

    }
}

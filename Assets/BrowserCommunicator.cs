using UnityEngine;
using System.Collections;

public class BrowserCommunicator : MonoBehaviour {

    IEnumerator Start()
    {
        Application.ExternalEval("alert('Finished Loading!');");
        yield break;
    }
}

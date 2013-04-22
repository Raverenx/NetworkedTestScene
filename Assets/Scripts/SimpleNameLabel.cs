using System.Collections;
using UnityEngine;

public class SimpleNameLabel : SimpleObjectLabel
{
    protected override void Start()
    {
        base.Start();
        name = "___";
        guiText.text = name;
        if (networkView.isMine)
        {
            StartCoroutine(SyncName());
        }
    }

    private IEnumerator SyncName()
    {
        while (true)
        {
            networkView.RPC("UpdateName", RPCMode.All, PlayerPrefs.GetString("username", "default_username"), PlayerPrefs.GetInt("teamNumber"));
            yield return new WaitForSeconds(5.0f);
        }
    }

    [RPC]
    void UpdateName(string name, int team)
    {
        this.name = name;
        if (guiText)
        {
            guiText.text = this.name;
            if (team == PlayerPrefs.GetInt("teamNumber", team))
            {
                guiText.material.color = Color.green;
            }
            else
            {
                guiText.material.color = Color.red;
            }
        }
    }

    string name;
}
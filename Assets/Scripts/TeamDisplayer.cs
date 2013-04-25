using UnityEngine;
using System.Collections.Generic;

public class TeamDisplayer : MonoBehaviour
{

    List<string> teamOneUsers = new List<string>();
    List<string> teamTwoUsers = new List<string>();


    void Update()
    {
        teamOneUsers = new List<string>();
        teamTwoUsers = new List<string>();
        var allPlayers = (Player[])Object.FindObjectsOfType(typeof(Player));
        foreach (Player player in allPlayers)
        {
            if (!player.GetComponentInChildren<GUIText>()) return;
            if (player.team == 1)
            {
                teamOneUsers.Add(player.GetComponentInChildren<GUIText>().text);
            }
            else if (player.team == 2)
            {
                var name = player.GetComponentInChildren<GUIText>().text;
                teamTwoUsers.Add(name);
            }
        }
    }

    void OnGUI()
    {
        GUI.skin = Util.ISEGUISkin;
        var leftRect = new Rect(10,50, Screen.height - 100, 300);
        GUI.BeginGroup(leftRect);
        GUILayout.BeginVertical();
        GUI.color = Color.white;
        GUILayout.Label("Team One:");
        foreach (string name in teamOneUsers)
        {
            GUILayout.Label(name);
        }
        GUILayout.Space(50);
        GUILayout.Label("Team Two:");

        foreach (string name in teamTwoUsers)
        {
            GUILayout.Label(name);
        }
        GUILayout.EndVertical();
        GUI.EndGroup();
    }
}


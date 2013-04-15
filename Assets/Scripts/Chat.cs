// Converted from UnityScript to C# at http://www.M2H.nl/files/js_to_c.php - by Mike Hergaarden
// Do test the code! You usually need to change a few small bits.

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[DefaultSceneObject("Chat", "Chat", hasNetworkView: true)]
public class Chat : MonoBehaviour
{
    GUISkin skin;
    bool visible = false;
    private string inputField = "";
    private List<ChatEntry> newEntries = new List<ChatEntry>();
    private List<ChatEntry> oldEntries = new List<ChatEntry>();
    private Vector2 scrollPosition;
    GUIStyle style;
    float chatWidth = Screen.width / 1.7f;
    float chatHeight = 140;
    Rect chatArea;
    private bool visibleNextFrame = false;
    private bool invisibleNextFrame = false;
    private bool submitNextFrame = false;

    public bool enableChatShadow = true;

    class ChatEntry
    {
        public string sender = "";
        public string text = "";
        public bool mine = true;
        public float age;
    }

    void Start()
    {
        skin = Resources.Load("ISEGUISkin") as GUISkin;
        if (!skin) Debug.Log("Skin is null.");
    }

    void Update()
    {
        chatWidth = Screen.width / 2.4f;
        chatHeight = Screen.height * .31f;
        chatArea = new Rect(0.25f * Screen.width + 10, Screen.height - chatHeight - 25, chatWidth, chatHeight);

        for (int g = newEntries.Count - 1; g >= 0; g--)
        {
            if (newEntries[g].age >= 7)
            {
                while (g >= 0)
                {
                    oldEntries.Add(newEntries[g]);
                    newEntries.RemoveAt(g);
                    g--;
                }
            }
            else
            {
                newEntries[g].age += Time.deltaTime;
            }
        }

        if (invisibleNextFrame) { visible = false; invisibleNextFrame = false; }
        if (visibleNextFrame) { visible = true; visibleNextFrame = false; }
        if (submitNextFrame) { SubmitChatMessage(); submitNextFrame = false; }
    }

    private void FocusChatInput()
    {
        //GameObject.Find("MainCamera").GetComponent<RegularCamera>().DisableScrolling();
        visibleNextFrame = true; scrollPosition.y = 100000;
    }

    private void UnfocusChatInput()
    {
    //    GameObject.Find("MainCamera").GetComponent<RegularCamera>().EnableScrolling();
        invisibleNextFrame = true;
    }

    void OnGUI()
    {

        GUI.skin = skin;
        Event e = Event.current;
        if (e.type == EventType.MouseDown && !chatArea.Contains(e.mousePosition))
        {
            UnfocusChatInput();
        }
        if (e.type == EventType.KeyDown)
        {
            if (e.keyCode == KeyCode.KeypadEnter || e.keyCode == KeyCode.Return)
            {
                if (visible)
                {
                    if (inputField != "")
                        submitNextFrame = true;
                }
                else
                    FocusChatInput();
            }
            else if (e.keyCode == KeyCode.Escape)
                UnfocusChatInput();
        }

        GUILayout.BeginArea(chatArea);
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, "box");
        GUILayout.BeginVertical();

        if (visible)
        {
            foreach (ChatEntry entry in oldEntries)
            {
                PrintChatEntry(entry);
            }
        }
        foreach (ChatEntry entry in newEntries)
        {
            PrintChatEntry(entry, true);
        }
        GUILayout.EndVertical();
        GUILayout.EndScrollView();
        if (visible)
        {
            GUI.SetNextControlName("inputField");
            inputField = GUILayout.TextField(inputField, 100);
            GUI.FocusControl("inputField");
        }
        GUILayout.EndArea();
    }

    private void PrintChatEntry(ChatEntry entry, bool fade = false)
    {
        if (fade)
        {
            if (entry.age > 5)
            {
                float alpha = 1 - ((entry.age - 5) / 2f);
                var color = GUI.color;
                color.a = alpha;
                GUI.color = color;
            }
            else
            {
                var color = GUI.color;
                color.a = 1f;
                GUI.color = color;
            }
        }
        if (enableChatShadow)
        {
            GUILayout.Label("<color=black>" + entry.sender + ": " + entry.text + "</color>", "smallLabel");
            var lastRect = GUILayoutUtility.GetLastRect();
            lastRect.x -= 1f; lastRect.y -= 1f;
            GUI.Label(lastRect, "<color=red>" + entry.sender + "</color>" + ": " + entry.text, "smallLabel");
        }
        else
        {
            GUILayout.Label("<color=red>" + entry.sender + "</color>" + ": " + entry.text, "smallLabel");

        }

    }

    private void SubmitChatMessage()
    {
        ApplyGlobalChatText(inputField, 1, GetUsername());
        networkView.RPC("ApplyGlobalChatText", RPCMode.Others, inputField, 0, GetUsername());
        inputField = "";
    }

    string GetUsername()
    {
        return PlayerPrefs.GetString("username", "default_username");
    }

    [RPC]
    void ApplyGlobalChatText(string str, int mine, string senderName)
    {
        var entry = new ChatEntry();
        entry.sender = senderName;
        entry.text = str;
        if (mine == 1) entry.mine = true;
        else entry.mine = false;
        entry.age = 0;

        newEntries.Add(entry);
        scrollPosition.y = 1000000;
    }
}
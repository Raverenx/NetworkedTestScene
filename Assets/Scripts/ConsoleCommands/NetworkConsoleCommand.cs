using UnityEngine;
using System.Linq;

/// <summary>
/// Represents a console command that is relayed to the server and then executed.
/// </summary>
public abstract class NetworkConsoleCommand
{
    /// <summary>
    /// Relays this command to the server
    /// </summary>
    public void TryExecute(params string[] parameters)
    {
        string allParams = "";
        foreach (string s in parameters)
        {
            allParams += s + " ";
        }
        DebugGUI.Main.networkView.RPCToServer(DebugGUI.Main, "ExecuteNetworkConsoleCommand", this.Name, allParams);

    }

    public void CommitExecute(GameObject player, string parameters)
    {
        ExecuteCommand(player, parameters.Split(' '));
    }
    public abstract void ExecuteCommand(GameObject invokerObject, params string[] parameters);
    public abstract void ApplyLocalEffects(GameObject invokerObject, params string[] parameters);
    public abstract string Name { get; }
    public abstract string HelpMessage { get; }
   
}
using UnityEngine;
using System.Linq;

[DefaultSceneObject("CommandLineReader")]
public class CommandLineReader : MonoBehaviour
{
    void Awake()
    {
        var args = System.Environment.GetCommandLineArgs();

        if (args.ToList().Contains("StartHeadlessServer"))
        {
            StartHeadlessServer(int.Parse(args[3]), args[4], args[5]);
        }
    }

    void StartHeadlessServer(int port, string name, string description)
    {
        this.gameObject.AddComponent<StrifeServer>().StartHeadlessServer(port, name, description);
    }
}
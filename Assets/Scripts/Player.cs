using UnityEngine;

public class Player : MonoBehaviour
{
    public int team = 0;

    [RPC]
    public void ChangeColor(float r, float g, float b)
    {
        var renderers = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = new Color(r, g, b);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo msg)
    {
        stream.Serialize(ref team);
    }
}
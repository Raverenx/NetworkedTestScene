using UnityEngine;

public class Player : MonoBehaviour
{
    [RPC]
    public void ChangeColor(float r, float g, float b)
    {
        var renderers = this.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            renderer.material.color = new Color(r, g, b);
        }
    }
}
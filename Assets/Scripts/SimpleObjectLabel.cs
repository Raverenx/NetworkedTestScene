using UnityEngine;
using System;

/// <summary>
/// Similar to ObjectLabel, but doesn't have a healthbar. Used for static objects that don't have character stats.
/// </summary>
public class SimpleObjectLabel : MonoBehaviour
{

    public Transform target;		// Object that this label should follow
    float height;	// Height above player to display label
    public Vector3 offset;	// Units in world space to offset; 1 unit above object by default
    private Camera cam;
    private Transform thisTransform;
    private Transform camTransform;
    Vector3 screenOffset;
    public string labelText;
    private float maxDisplayDistance = 40.0f;


    protected virtual void Start()
    {
        thisTransform = transform;
        cam = Camera.main;
        camTransform = cam.transform;
        offset = new Vector3(0, 3f, 0);
        if (!target)
            target = transform.root;

        if (!guiText)
        {
            this.gameObject.AddComponent<GUIText>();
            guiText.font = Resources.Load("Fonts/OFLGoudyStMTT") as Font;
            guiText.fontSize = 16;
            guiText.anchor = TextAnchor.UpperCenter;
            guiText.material.color = new Color(230, 232, 250);
        }

    }

    protected virtual void LateUpdate()
    {
        if (Vector3.Distance(camTransform.position, transform.root.position) > maxDisplayDistance
            || Vector3.Dot(camTransform.forward, target.position - camTransform.position) < 0)
        {
            DisableLabels();
            return;
        }
        EnableLabels();
        enabled = true;

        TrackPlayer();

    }

    protected void DisableLabels()
    {
        guiText.enabled = false;
    }

    protected void EnableLabels()
    {
        guiText.enabled = true;

    }

    protected void DestroyLabels()
    {
        Destroy(this.gameObject);
    }

    protected virtual void TrackPlayer()
    {
        thisTransform.position = cam.WorldToViewportPoint(target.position + offset);

        FadeTextColor();

    }

    protected virtual void FadeTextColor()
    {
        var color = guiText.material.color;
        color.a = 1f - (Vector3.Distance(camTransform.position, transform.root.position) / maxDisplayDistance);
        guiText.material.color = color;
    }
}

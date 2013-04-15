using UnityEngine;

public class SimpleNameLabel : SimpleObjectLabel
{
    protected override void Start()
    {
        base.Start();
        guiText.text = target.name;
    }
}
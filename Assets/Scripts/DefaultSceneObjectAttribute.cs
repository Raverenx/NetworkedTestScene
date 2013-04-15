using System;
using UnityEngine;
using System.Reflection;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class DefaultSceneObjectAttribute : Attribute
{
    /// <summary>
    /// The name for the GameObject to create
    /// </summary>
    public string gameObjectName;

    public bool hasNetworkView;

    /// <summary>
    /// The name of the prefab if there is one associated.
    /// </summary>
    public string prefabName;

    public DefaultSceneObjectAttribute(string gameObjectName, string prefabName = null, bool hasNetworkView = false)
    {
        this.gameObjectName = gameObjectName;
        this.prefabName = prefabName;
        this.hasNetworkView = hasNetworkView;
    }
}
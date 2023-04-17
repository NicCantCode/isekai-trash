/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlight : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToHighlight;

    private Dictionary<string, int> defaultLayers;
    private int outlineLayer;

    private void Awake()
    {
        outlineLayer = LayerMask.NameToLayer("Outline");
        defaultLayers = new Dictionary<string, int>();
        
        foreach (var obj in objectsToHighlight)
        {
            defaultLayers.Add(obj.name, obj.layer);
        }
    }

    private void OnMouseEnter()
    {
        foreach (var obj in objectsToHighlight)
        {
            obj.layer = outlineLayer;
        }
    }

    private void OnMouseExit()
    {
        foreach (var obj in objectsToHighlight)
        {
            obj.layer = defaultLayers[obj.name];
        }
    }
    
    
}

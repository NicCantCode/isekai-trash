/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Highlight : MonoBehaviour
{
    [SerializeField] private List<GameObject> objectsToHighlight;
    [SerializeField] private InputAction mouseInput; // Will eventually move all input to its own manager class

    private Dictionary<string, int> defaultLayers;
    private int outlineLayer;
    private NPCDialogController _npcDialogController;

    private void Awake()
    {
        _npcDialogController = GetComponent<NPCDialogController>();

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

    private void OnMouseOver()
    {
        if (mouseInput.ReadValue<float>() == 1f)
        {
            _npcDialogController.OpenDialog();
        }
    }

    private void OnMouseExit()
    {
        foreach (var obj in objectsToHighlight)
        {
            obj.layer = defaultLayers[obj.name];
        }
    }
    
    private void OnEnable()
    {
        mouseInput.Enable(); // Will eventually move all input to its own manager class
    }

    private void OnDisable()
    {
        mouseInput.Disable(); // Will eventually move all input to its own manager class
    }
}

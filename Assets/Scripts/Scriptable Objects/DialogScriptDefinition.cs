/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "New Dialog Script", menuName = "Scriptable Object/Dialog Script", order = 1)]
public class DialogScriptDefinition : SerializedScriptableObject
{
    public Dictionary<int, DialogLine> dialogLines;
}

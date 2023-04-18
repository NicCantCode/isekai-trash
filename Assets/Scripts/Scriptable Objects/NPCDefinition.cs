/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using UnityEngine;

[CreateAssetMenu(fileName = "New NPC", menuName = "Scriptable Objects/NPC", order = 1)]
public class NPCDefinition : ScriptableObject
{
    public string characterName;
    public GameObject prefab;
    public DialogScriptDefinition dialogScript;
}

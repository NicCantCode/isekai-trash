/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using UnityEngine;

public class NPCSpawnerController : MonoBehaviour
{
    [SerializeField] private NPCDefinition NPCDefinition;

    private void Start()
    {
        SpawnNPC();
    }

    private void SpawnNPC()
    {
        GameObject npc = Instantiate(NPCDefinition.prefab, transform.position, Quaternion.identity, transform);
    }
}

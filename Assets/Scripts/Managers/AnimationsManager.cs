/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using UnityEngine;
using UnityEngine.AI;

public class AnimationsManager : MonoBehaviour
{
    // Singleton Property
    public static AnimationsManager Instance { get; private set; }
    
    [Header("Player Animation Dependencies")]
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private PlayerController playerController;

    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int SpeedMultiplier = Animator.StringToHash("speedMultiplier");

    private void Awake()
    {
        // Singleton Logic
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void Update()
    {
        HandleAnimationState();
    }

    private void HandleAnimationState()
    {
        var velocity = agent.velocity.magnitude;
        
        playerAnimator.SetFloat(SpeedMultiplier, playerController.Speed / 10);
        playerAnimator.SetBool(IsRunning, velocity > 0);
    }
}

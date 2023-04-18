/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Singleton Property
    public static InputManager Instance { get; private set; }
    
    [Header("Inputs")]
    [SerializeField] private InputAction movementInput;
    [SerializeField] private InputAction cameraZoomInput;

    // Public Properties
    public InputAction MovementInput => movementInput;
    public InputAction CameraZoomInput => cameraZoomInput;

    private void Awake()
    {
        // Singleton Logic
        if (Instance != null && Instance != this)
            Destroy(this);
        else
            Instance = this;
    }

    private void OnEnable()
    {
        EnableInputs();
    }

    private void OnDisable()
    {
        DisableInputs();
    }

    private void EnableInputs()
    {
        movementInput.Enable();
        cameraZoomInput.Enable();
    }

    private void DisableInputs()
    {
        movementInput.Disable();
        cameraZoomInput.Disable();
    }
}

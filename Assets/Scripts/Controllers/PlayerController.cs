/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    [Header("Debug")]
    [SerializeField] private TextMeshProUGUI debugSpeedText;
    [SerializeField] private TextMeshProUGUI debugZoomText;
    [SerializeField] private float speed = 7.5f;
    [SerializeField] private InputAction debugSpeedUp;
    [SerializeField] private InputAction debugSpeedDown;
    
    [Header("Variables")]
    [SerializeField] private InputAction movementInput;
    [SerializeField] private InputAction cameraZoomInput;
    [SerializeField] private LayerMask navigableMask;
    [SerializeField] private CinemachineVirtualCamera virtualFollowCamera;
    [SerializeField] private Vector2 zoomConstraints;
    [SerializeField] private Vector2 stoppingDistanceHysteresis;
    
    private NavMeshAgent _agent;
    private Camera _followCamera;
    private CinemachineFramingTransposer _framingTransposer;
    private Animator _playerAnimator;
    
    private static readonly int IsRunning = Animator.StringToHash("isRunning");
    private static readonly int SpeedMultiplier = Animator.StringToHash("speedMultiplier");

    private void Start()
    {
        _playerAnimator = GetComponent<Animator>();
        _framingTransposer = virtualFollowCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _followCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        debugSpeedText.text = "Speed: " + speed;
        debugZoomText.text = "Zoom: " + _framingTransposer.m_CameraDistance;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
        HandleAnimationState();
        
        // Debug
        HandleSpeedDebug();
        _agent.speed = speed; // Will later be handled by an update event
    }

    private void OnEnable()
    {
        movementInput.Enable();
        cameraZoomInput.Enable();
        
        // Debug
        debugSpeedUp.Enable();
        debugSpeedDown.Enable();
    }

    private void OnDisable()
    {
        movementInput.Disable();
        cameraZoomInput.Disable();
        
        // Debug
        debugSpeedUp.Disable();
        debugSpeedDown.Disable();
    }

    private void HandleAnimationState()
    {
        float velocity = _agent.velocity.magnitude;
        
        _playerAnimator.SetFloat(SpeedMultiplier, speed / 10);

        if (velocity > 0)
        {
            _playerAnimator.SetBool(IsRunning, true);
        }
        else
        {
            _playerAnimator.SetBool(IsRunning, false);
        }
    }

    private void HandleSpeedDebug()
    {
        if (debugSpeedUp.triggered)
        {
            speed += 0.5f;
            debugSpeedText.text = "Speed: " + speed;
        }

        if (debugSpeedDown.triggered)
        {
            speed -= 0.5f;
            debugSpeedText.text = "Speed: " + speed;
        }
    }

    private void HandleZoom()
    {
        float zoom = cameraZoomInput.ReadValue<Vector2>().y;
        
        if (zoom < 0)
        {
            _framingTransposer.m_CameraDistance = Mathf.Clamp(_framingTransposer.m_CameraDistance + 0.5f, zoomConstraints.x, zoomConstraints.y);
            debugZoomText.text = "Zoom: " + _framingTransposer.m_CameraDistance;
        }

        if (zoom > 0)
        {
            _framingTransposer.m_CameraDistance = Mathf.Clamp(_framingTransposer.m_CameraDistance - 0.5f, zoomConstraints.x, zoomConstraints.y);
            debugZoomText.text = "Zoom: " + _framingTransposer.m_CameraDistance;
        }
    }

    private void HandleMovement()
    {
        if (movementInput.ReadValue<float>() == 1f)
        {
            Ray ray = _followCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out var hit, 100, navigableMask))
            {
                Move(hit.point);
            }
        }
    }

    private void Move(Vector3 destination)
    {
        float distanceToDestination = Vector3.Distance(transform.position, destination);
        
        if (distanceToDestination <= stoppingDistanceHysteresis.x)
            _agent.stoppingDistance = stoppingDistanceHysteresis.y;
        
        if (distanceToDestination >= stoppingDistanceHysteresis.y)
            _agent.stoppingDistance = stoppingDistanceHysteresis.x;
        
        _agent.SetDestination(destination);
    }
}

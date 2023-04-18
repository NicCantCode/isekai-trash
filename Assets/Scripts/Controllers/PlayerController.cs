/*
 * Copyright (c) NicCantCode
 * https://nichal.us/
 */

using Cinemachine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerController : MonoBehaviour
{
    
    [Header("Dependencies")]
    [SerializeField] private CinemachineVirtualCamera virtualFollowCamera;
    
    [Header("Settings")]
    [SerializeField] private LayerMask navigableMask;
    [SerializeField] private Vector2 zoomConstraints;
    [SerializeField] private Vector2 stoppingDistanceHysteresis;
    
    [Header("Debug")]
    [SerializeField] private float speed = 7.5f;

    public float Speed => speed;
    
    private NavMeshAgent _agent;
    private Camera _followCamera;
    private CinemachineFramingTransposer _framingTransposer;

    private void Awake()
    {
        _framingTransposer = virtualFollowCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _followCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        HandlePlayerControls();
        
        _agent.speed = speed; // Will later be handled by an update event
    }

    private void HandlePlayerControls()
    {
        HandleMovement();
        HandleZoom();
    }

    private void HandleZoom()
    {
        var zoom = InputManager.Instance.CameraZoomInput.ReadValue<Vector2>().y;

        _framingTransposer.m_CameraDistance = zoom switch
        {
            < 0 => Mathf.Clamp(_framingTransposer.m_CameraDistance + 0.5f, zoomConstraints.x, zoomConstraints.y),
            > 0 => Mathf.Clamp(_framingTransposer.m_CameraDistance - 0.5f, zoomConstraints.x, zoomConstraints.y),
            _ => _framingTransposer.m_CameraDistance
        };
    }

    private void HandleMovement()
    {
        if (InputManager.Instance.MovementInput.ReadValue<float>() != 1f) return;
        
        var ray = _followCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out var hit, 100, navigableMask))
        {
            Move(hit.point);
        }
    }

    private void Move(Vector3 destination)
    {
        var distanceToDestination = Vector3.Distance(transform.position, destination);
        
        if (distanceToDestination <= stoppingDistanceHysteresis.x)
            _agent.stoppingDistance = stoppingDistanceHysteresis.y;
        
        if (distanceToDestination >= stoppingDistanceHysteresis.y)
            _agent.stoppingDistance = stoppingDistanceHysteresis.x;
        
        _agent.SetDestination(destination);
    }
}

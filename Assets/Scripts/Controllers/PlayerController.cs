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
    [SerializeField] private InputAction debugSpeedUp = new InputAction();
    [SerializeField] private InputAction debugSpeedDown = new InputAction();
    
    [Header("Variables")]
    [SerializeField] private InputAction movementInput = new InputAction();
    [SerializeField] private InputAction cameraZoomInput = new InputAction();
    [SerializeField] private LayerMask navigableMask = new LayerMask();
    [SerializeField] private CinemachineVirtualCamera _virtualFollowCamera;
    [SerializeField] private Vector2 zoomConstraints = new Vector2();
    
    private NavMeshAgent _agent = null;
    private Camera _followCamera = null;
    private CinemachineFramingTransposer _framingTransposer = null;

    private void Start()
    {
        _framingTransposer = _virtualFollowCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _followCamera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
        debugSpeedText.text = "Speed: " + speed;
        debugZoomText.text = "Zoom: " + _framingTransposer.m_CameraDistance;
    }

    private void Update()
    {
        HandleMovement();
        HandleZoom();
        
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
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100, navigableMask))
            {
                Move(hit.point);
            }
        }
    }

    private void Move(Vector3 destination)
    {
        _agent.SetDestination(destination);
    }
}

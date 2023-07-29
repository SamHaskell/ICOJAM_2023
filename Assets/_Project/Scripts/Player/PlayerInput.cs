using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : PlayerSystem
{
    private PlayerControls _playerControls;
    private Vector3 _lookAtTarget;
    [SerializeField] private LayerMask _mousePickingLayerMask;
    [SerializeField] private GameObject _directionIndicator;
    protected override void Awake()
    {
        base.Awake();
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        _playerControls.Player.Jump.started += OnJumpPress;
        _playerControls.Player.Jump.canceled += OnJumpRelease;
        _playerControls.Player.Fire.started += OnShootPerformed;
        _playerControls.Player.Fire.canceled += OnShootCancelled;

        GameManager.OnGamePause += OnDisable;
        TimeManager.OnTimeStop += OnDisable;
        GameManager.OnGameResume += OnEnable;
        TimeManager.OnTimeBegin += OnEnable;
    }

    void OnEnable()
    {
        _directionIndicator.SetActive(true);
        _playerControls.Enable();
    }

    void OnDisable()
    {
        _directionIndicator.SetActive(false);
        _playerControls.Disable();
    }

    void Update()
    {
        Player.PlayerData.Events.MoveCommand?.Invoke(_playerControls.Player.Move.ReadValue<Vector2>());
        Vector2 mousePosition = _playerControls.Player.Look.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f, _mousePickingLayerMask)) {
            Player.PlayerData.Events.LookAtCommand?.Invoke(hit.point);
            _lookAtTarget = hit.point;
        }
    }

    void OnJumpPress(InputAction.CallbackContext ctx)
    {
        Player.PlayerData.Events.JumpChargeCommand?.Invoke();
        Debug.Log("Lol!");
    }

    void OnShootPerformed(InputAction.CallbackContext ctx)
    {
        Player.PlayerData.Events.ShootCommand?.Invoke();
    }

    void OnShootCancelled(InputAction.CallbackContext ctx)
    {
        Player.PlayerData.Events.ShootStopCommand?.Invoke();
    }

    void OnJumpRelease(InputAction.CallbackContext ctx)
    {
        Player.PlayerData.Events.JumpReleaseCommand?.Invoke(_lookAtTarget);
    }
}
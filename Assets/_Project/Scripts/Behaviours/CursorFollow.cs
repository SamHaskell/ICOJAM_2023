using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorFollow : MonoBehaviour
{
    [SerializeField] LayerMask _mousePickingLayerMask;
    private PlayerControls _playerControls;
    [SerializeField] private float _rotationFrequency;
    void Awake()
    {
        _playerControls = new PlayerControls();
        _playerControls.Enable();

        GameManager.OnGamePause += OnDisable;
        GameManager.OnGameResume += OnEnable;
    }

    void OnEnable()
    {
        _playerControls.Enable();
    }

    void OnDisable()
    {
        _playerControls.Disable();
    }

    void Update()
    {
        Vector2 mousePosition = _playerControls.Player.Look.ReadValue<Vector2>();
        Ray ray = Camera.main.ScreenPointToRay(mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f, _mousePickingLayerMask)) {
            transform.position = hit.point;
        }

        transform.Rotate(0.0f, 360.0f * _rotationFrequency * Time.deltaTime, 0.0f);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; private set; }
    private PlayerInput _playerInput;
    private PlayerControls _playerControls;
    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(this);
        }

        _playerControls = new PlayerControls();
        _playerControls.Enable();

        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnEnable()
    {
        _playerControls.Enable();
        Cursor.lockState = CursorLockMode.Confined;
    }

    void OnDisable()
    {
        _playerControls.Disable();
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {

    }
}
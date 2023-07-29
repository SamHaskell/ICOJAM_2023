using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : PlayerSystem
{   
    [SerializeField] private float _baseMoveSpeed;
    [SerializeField] private Transform _playerBody;
    [SerializeField] private float _gravityStrength;
    [SerializeField] private float _groundedDownwardVelocity;
    [SerializeField] private float _jumpChargeRate;
    [SerializeField] private float _jumpMinHeight;
    [SerializeField] private float _jumpMaxHeight;
    [SerializeField] private float _jumpMinDistance;
    [SerializeField] private float _jumpMaxDistance;
    [SerializeField] [Range(0.0f, 5.0f)] private float _groundCheckRadius;
    [SerializeField] private LayerMask _groundCheckLayerMask;
    [SerializeField] [Range(0.0f, 0.1f)] private float _skinWidth;
    [SerializeField] [Range(0.0f, 1.0f)] private float _moveDeadzone;
    
    private Vector2 _rawMoveInput;
    private Vector3 _lookAtInput;

    private Vector3 _moveDirection;
    private Vector3 _transientVelocity;
    private Vector3 _persistantVelocity;
    private float _jumpPower;

    private bool _isGrounded;
    private bool _isJumping = false;
    private bool _isChargingJump;

    private Rigidbody _rb;
    protected override void Awake()
    {
        base.Awake();
        Player.PlayerData.Events.MoveCommand += GetMoveInput;
        Player.PlayerData.Events.LookAtCommand += GetLookAtInput;
        Player.PlayerData.Events.JumpChargeCommand += JumpCharge;
        Player.PlayerData.Events.JumpReleaseCommand += JumpRelease;
        _rb = GetComponentInChildren<Rigidbody>();
    }

    void Update()
    {
        CollisionChecks();

        _transientVelocity = Vector3.zero;
        Vector3 lookDirection = _lookAtInput - transform.position;
        lookDirection.y = 0.0f;
        lookDirection.Normalize();
        Quaternion lookRotation = Quaternion.LookRotation(lookDirection);
        _playerBody.rotation = lookRotation;

        _persistantVelocity += Vector3.down * Time.deltaTime * _gravityStrength;
        if (_isGrounded && _persistantVelocity.y < 0.0f) {
            _persistantVelocity.x = 0.0f;
            _persistantVelocity.z = 0.0f;
            _persistantVelocity.y = Mathf.Clamp(_persistantVelocity.y, -_groundedDownwardVelocity, _persistantVelocity.y);
        }

        if (!_isJumping) {
            if (_isChargingJump) {
                _jumpPower += Time.deltaTime * _jumpChargeRate;
                _jumpPower = Mathf.Clamp(_jumpPower, 0.0f, 1.0f);
            } else {
                _jumpPower = 0;
            }

            _moveDirection = Camera.main.transform.TransformDirection(_rawMoveInput.x, 0.0f, _rawMoveInput.y);
            _moveDirection.y = 0.0f;
            _moveDirection.Normalize();
            _transientVelocity = _moveDirection * _baseMoveSpeed;
        }

        _rb.velocity = _persistantVelocity + _transientVelocity;
    }

    void CollisionChecks()
    {
        RaycastHit hit;
        if (Physics.SphereCast(
                transform.position + (Vector3.up * (_groundCheckRadius + _skinWidth)),
                _groundCheckRadius,
                Vector3.down, 
                out hit, 
                0.05f + _groundCheckRadius,
                _groundCheckLayerMask)) 
        {
            if (!_isGrounded && _isJumping) {
                Player.PlayerData.Events.OnPlayerLand?.Invoke();
            }
            _isGrounded = true;
            if (_persistantVelocity.y < 0.0f) {
                _isJumping = false;
            }
        } 
        else 
        {
            _isGrounded = false;
        }
    }

    void GetMoveInput(Vector2 moveInput)
    {
        _rawMoveInput = moveInput;
    }

    void GetLookAtInput(Vector3 lookAtInput)
    {
        _lookAtInput = lookAtInput;
    }

    void JumpCharge()
    {
        if (!_isGrounded || _isJumping) {
            return;
        }
        _isChargingJump = true;
    }

    void JumpRelease(Vector3 target)
    {
        if (!_isGrounded) {
            return;
        }
        _isJumping = true;
        _isChargingJump = false;
        float jumpHeight = Mathf.Lerp(_jumpMinHeight, _jumpMaxHeight, _jumpPower);
        _persistantVelocity.y = Mathf.Sqrt(2.0f * _gravityStrength * jumpHeight);
        float flightTime = 2.0f * _persistantVelocity.y / _gravityStrength;
        float planarSpeed = Mathf.Lerp(_jumpMinDistance, _jumpMaxDistance, _jumpPower) / flightTime;

        Vector3 targetDisplacement = _lookAtInput - transform.position;
        _persistantVelocity.x = targetDisplacement.normalized.x * planarSpeed;
        _persistantVelocity.z = targetDisplacement.normalized.z * planarSpeed;

        Player.PlayerData.Events.OnPlayerJump?.Invoke();
    }
}

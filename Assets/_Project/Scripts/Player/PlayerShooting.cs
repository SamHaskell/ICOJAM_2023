using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : PlayerSystem
{
    private bool _canShoot;
    private Vector3 _lookAtInput;
    private Vector3 _lookDirection;
    private float _timeSinceLastShot;
    [SerializeField] private int _maxAmmo;
    [SerializeField] private int _RPM;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Transform _bulletSpawnTransform;
    private bool _tryShoot;
    private int _remainingAmmo;
    protected override void Awake()
    {
        base.Awake();
        Player.PlayerData.Events.LookAtCommand += GetLookAtInput;
        Player.PlayerData.Events.OnPlayerJump += OnJump;
        Player.PlayerData.Events.OnPlayerLand += OnLand;
        Player.PlayerData.Events.ShootCommand += () => { _tryShoot = true; };
        Player.PlayerData.Events.ShootStopCommand += () => { _tryShoot = false; };
        _canShoot = true;
        _remainingAmmo = _maxAmmo;
        _timeSinceLastShot = 0;
        _tryShoot = false;
    }

    void Update()
    {
        _lookDirection = _lookAtInput - transform.position;
        _lookDirection.y = 0;
        _lookDirection.Normalize();
        _timeSinceLastShot += Time.deltaTime;

        if (_tryShoot)
        {
            TryShoot();
        }
    }

    void OnJump()
    {
        _canShoot = false;
    }

    void OnLand()
    {
        _canShoot = true;
    }

    void TryShoot()
    {
        if (!_canShoot) {
            return;
        }
        if (_timeSinceLastShot > (60.0f / (float)_RPM)) {
            if (_remainingAmmo > 0) {
                Shoot();
                _timeSinceLastShot = 0.0f;
            } else {
                Reload();
            }
        }
    }

    void Shoot()
    {
        Instantiate(_bulletPrefab, _bulletSpawnTransform.position, Quaternion.LookRotation(_lookDirection));
        _remainingAmmo --;
    }

    void Reload()
    {
        _remainingAmmo = _maxAmmo;
    }

    void GetLookAtInput(Vector3 lookAtInput)
    {
        _lookAtInput = lookAtInput;
    }
}

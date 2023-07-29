using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private float _lifetime;
    [SerializeField] private int _damage;
    private float _deathTimer;
    void Awake()
    {
        _deathTimer = _lifetime;
    }

    void Update()
    {
        transform.Translate(Vector3.forward * _bulletSpeed * Time.deltaTime);
        _deathTimer -= Time.deltaTime;
        if (_deathTimer < 0.0f) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        IHealth health;
        if (other.transform.root.TryGetComponent<IHealth>(out health)) {
            Debug.Log("Bullet Hit!");
            health.ApplyDamage(_damage);
        }
        Destroy(gameObject);
    }
}

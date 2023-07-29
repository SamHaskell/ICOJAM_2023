using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, IHealth
{
    [SerializeField] int _maxHealth;
    [SerializeField] int _baseMoveSpeed;
    [SerializeField] int _experience;
    [SerializeField] float _rotationFrequency;
    public int Experience { get {return _experience;} }
    private int _currentHealth;
    public static event Action<Enemy> OnAnyEnemyDeath; 
    void Awake()
    {
        _currentHealth = _maxHealth;
    }
    void Update()
    {
        transform.Rotate(0.0f, 360.0f * _rotationFrequency * Time.deltaTime, 0.0f);
    }
    public void ApplyDamage(int amount)
    {
        Debug.Log("Ow!");
        _currentHealth -= amount;
        if (_currentHealth <= 0) {
            OnDeath();
        }
    }
    public void OnDeath()
    {
        OnAnyEnemyDeath?.Invoke(this);
        Destroy(gameObject);
    }
    public void ApplyHeal(int amount)
    {
        _currentHealth = Mathf.Clamp(_currentHealth + amount, _currentHealth, _maxHealth);
    }
}

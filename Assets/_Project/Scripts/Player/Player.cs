using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] public PlayerData PlayerData;
    public static event Action<Player> OnAnyPlayerDeath;
    [SerializeField] private int _experience;

    void Awake()
    {
        Enemy.OnAnyEnemyDeath += OnEnemyKill;
    }

    void OnEnemyKill(Enemy enemy)
    {
        _experience += enemy.Experience;
        Debug.Log(_experience);
    }

    public void Kill()
    {
        OnAnyPlayerDeath?.Invoke(this);
        PlayerData.Events.OnPlayerDeath?.Invoke();
    }

    public void RespawnAt(Transform tf)
    {
        Debug.Log("Respawning!");
        transform.position = tf.position;
        transform.rotation = tf.rotation;
    }
}

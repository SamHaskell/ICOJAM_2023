using System;
using UnityEngine;

[CreateAssetMenu]
public class PlayerData : ScriptableObject
{
    public PlayerEvents Events;
}

public struct PlayerEvents {
    public Action OnPlayerDeath;
    public Action OnPlayerSpawn;
    public Action OnPlayerJump;
    public Action OnPlayerLand;
    public Action<Vector2> MoveCommand;
    public Action<Vector3> LookAtCommand;
    public Action ShootCommand;
    public Action ShootStopCommand;
    public Action JumpChargeCommand;
    public Action<Vector3> JumpReleaseCommand;
};

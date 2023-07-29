using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealth
{
    public void ApplyDamage(int amount);
    public void ApplyHeal(int amount);
    void OnDeath();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBattle
{
    float AttackPower { get; }
    float DefencePower { get; }

    void Attact(IBattle target);
    void Defence(float damage);
}

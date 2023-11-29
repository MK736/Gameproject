using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damagee(Collider other, int hp, int atackPower);
    public void Death(Collider other, int hp);
}

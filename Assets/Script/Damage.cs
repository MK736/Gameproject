using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public void Damagee(Collider other, int atackPower);
    public void Death(Collider other, int hp);
}

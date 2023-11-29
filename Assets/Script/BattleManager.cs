using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour, IDamageable
{
    Player m_Player = null;
    Enemy m_Enemy = null;

    public bool EnemyDamage = false;

    public bool EnemyDeth = false;

    private void Awake()
    {
        m_Player = GetComponent<Player>();
        m_Enemy = GetComponent<Enemy>();
    }
    public int HpDown(int hp, int atackPower)
    {
        hp -= atackPower;
        return hp;
    }

    public int HpUp(int hp, int Healpower)
    {
        hp += Healpower;
        return hp;
    }

    // case0 = player -> enemy
    // case1 = enemy -> player

    public void Damagee(Collider other, int atackPower)
    {
        if (other.CompareTag("weapon"))
        {

            m_Enemy.enemyHp-= atackPower;
            EnemyDamage = true;
        }
        else if(other.CompareTag("Enemy"))
        {
            m_Player.g_PlayerHP -= atackPower;
        }

        if (m_Enemy.enemyHp < 0)
        {
            EnemyDeth = true;
        }
    }

    public void Death(Collider other, int hp)
    {
        if (other.CompareTag("weapon")&&hp == 0)
        {
            m_Enemy.Deth();
            m_Enemy.m_BoxCollider.enabled = false;
            m_Enemy.AtackBoxCollider.enabled = false;
        }
    }
}
